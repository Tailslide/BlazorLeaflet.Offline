using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorLeaflet.Offline.Services
{
    public class BlazorLeafletOfflineService
    {
		private IJSRuntime _jsRuntime;
		private DotNetObjectReference<BlazorLeafletOfflineService> objRef;
		public delegate void ElementClickedEventHandler(object sender, ElementClickedEventArgs e);
		public delegate void OpenContextMenuEventHandler(object sender, RawJsonEventArgs e);

		public BlazorLeafletOfflineService(IJSRuntime jSRuntime)
		{
			_jsRuntime = jSRuntime;
			objRef = DotNetObjectReference.Create(this);
		}

		public async Task LogVar (object var)
        {
			await _jsRuntime.InvokeVoidAsync("BlazorLeafletOffline.LogVar", var);
        }

		public async Task InitializeMap(string elementId, Settings settings = null)
		{
			if (settings == null) settings = new Settings();
			await _jsRuntime.InvokeVoidAsync("BlazorLeafletOffline.InitializeMap", objRef, elementId, settings );
		}
		public async Task ShowTileList()
		{
			await _jsRuntime.InvokeVoidAsync("BlazorLeafletOffline.ShowTileList");
		}
		public async Task Show()
		{
			await _jsRuntime.InvokeVoidAsync("BlazorLeafletOffline.Show");
		}


		public async Task AddMarker(Marker marker)
		{
			await _jsRuntime.InvokeVoidAsync("BlazorLeafletOffline.AddMarker", objRef, marker);
		}

		public event ElementClickedEventHandler ElementClicked;

		public event OpenContextMenuEventHandler OpenContextMenu;

		public async Task AddLayer(string layerName, string styleJson)
        {
			await _jsRuntime.InvokeVoidAsync("BlazorLeafletOffline.AddLayer", layerName, styleJson);
		}

		public async Task AddGeoJsonToLayer(string layerName, string geojsonFeature)
		{
			await _jsRuntime.InvokeVoidAsync("BlazorLeafletOffline.AddGeoJsonToLayer", layerName, geojsonFeature);
		}

		[JSInvokable]
		public Task<bool> JSCallBackElementClicked(string elementId)
		{
			ElementClicked?.Invoke(this, new ElementClickedEventArgs() { ElementId = elementId });
			return Task.FromResult(true);
		}


		[JSInvokable]
		public Task<bool> JSCallBackOpenContextMenu(string json)
		{
			//Console.Write("JSCallBackOpenContextMenu():");
			//Console.Write(json);
			OpenContextMenu?.Invoke(this, new RawJsonEventArgs() { Json = JsonDocument.Parse(json) });
			return Task.FromResult(true);
		}
	}
}
