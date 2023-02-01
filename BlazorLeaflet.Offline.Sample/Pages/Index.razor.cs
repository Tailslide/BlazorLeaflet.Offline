using BlazorLeaflet.Offline.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace BlazorLeaflet.Offline.Sample.Pages
{
    public partial class Index
    {
        [Inject]
        public BlazorLeafletOfflineService BlazorLeafletOffline { get; set; }

        TelerikContextMenu<MenuItem> TheContextMenu { get; set; }

        public List<MenuItem> MenuItems { get; set; }

        private float lat = 0F;
        private float lng = 0F;
        public class MenuItem
        {
            public string Text { get; set; }
            public string TelerikIcon { get; set; }
            public string MyImage { get; set; }
            public string MyIconClass { get; set; }
        }

        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }

        private int markerCount = 0;

        protected async Task OnItemClick(MenuItem item)
        {
            
            if (await Dialogs.ConfirmAsync($"TODO: Open dialog with Lat={lat} Long={lng}"))
            {
                markerCount++;
                string carsvg = $"<svg id='drive-{markerCount}' onclick='window.leafletElementClicked(\"drive-{markerCount}\");' xmlns='http://www.w3.org/2000/svg' style='position: fixed;right: 20px;' height='24px' viewBox='0 0 24 24' width='24px' fill='#000000'><path d='M0 0h24v24H0V0z' fill='none'/><path d='M18.92 6.01C18.72 5.42 18.16 5 17.5 5h-11c-.66 0-1.21.42-1.42 1.01L3 12v8c0 .55.45 1 1 1h1c.55 0 1-.45 1-1v-1h12v1c0 .55.45 1 1 1h1c.55 0 1-.45 1-1v-8l-2.08-5.99zM6.85 7h10.29l1.08 3.11H5.77L6.85 7zM19 17H5v-5h14v5z'/><circle cx='7.5' cy='14.5' r='1.5'/><circle cx='16.5' cy='14.5' r='1.5'/></svg>";
                string editsvg = $"<svg id='edit-{markerCount}' onclick='window.leafletElementClicked(\"edit-{markerCount}\");' xmlns='http://www.w3.org/2000/svg'  height='24px' viewBox='0 0 24 24' width='24px' fill='#000000'><path d='M0 0h24v24H0V0z' fill='none'/><path d='M14.06 9.02l.92.92L5.92 19H5v-.92l9.06-9.06M17.66 3c-.25 0-.51.1-.7.29l-1.83 1.83 3.75 3.75 1.83-1.83c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.2-.2-.45-.29-.71-.29zm-3.6 3.19L3 17.25V21h3.75L17.81 9.94l-3.75-3.75z'/></svg>";
                await BlazorLeafletOffline.AddMarker(new Marker()
                {
                    Lat = lat,
                    Long = lng,

                PopupHtml = $"WO-123456 *new*<br/><hr/>{editsvg}{carsvg}"
                }
                );
            }

        }

        public string Message { get; set; }
        private Random random = new Random();
        protected override async Task OnInitializedAsync()
        {
            BlazorLeafletOffline.ElementClicked += BlazorLeafletOffline_ElementClicked;
            BlazorLeafletOffline.OpenContextMenu += BlazorLeafletOffline_OpenContextMenu;
            MenuItems = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Text = "Add Asset",
                    TelerikIcon = "car"
                },
                new MenuItem()
                {
                    Text = "Add Reading",
                    TelerikIcon = "book"
                    //MyIconClass = "book",
                },
                new MenuItem()
                {
                Text = "Add Event",
                TelerikIcon="clipboard"
                //MyImage = "https://docs.telerik.com/blazor-ui/images/star.png"
                }
            };
            
            await ShowMap();
            //return base.OnInitializedAsync();
        }

        private void BlazorLeafletOffline_OpenContextMenu(object sender, RawJsonEventArgs e)
        {
            string asstring = e.Json.RootElement.ToString();

            var re = e.Json.RootElement;
            foreach (var x in re.EnumerateObject())
            {
                Console.WriteLine($"{x.Name}={x.Value.ToString()}");
            }
            lat = (float)re.EnumerateObject().Single(x => x.Name == "lat").Value.GetDouble();
            lng = (float)re.EnumerateObject().Single(x => x.Name == "lng").Value.GetDouble();
            var clientX = (float)re.EnumerateObject().Single(x => x.Name == "clientX").Value.GetDouble();
            var clientY = (float)re.EnumerateObject().Single(x => x.Name == "clientY").Value.GetDouble();
            bool touched = re.EnumerateObject().Single(x => x.Name == "touchPress").Value.GetBoolean();
            if (touched)
            {
                //Console.WriteLine("Blazor TODO: open a context menu for " + asstring);
                TheContextMenu.ShowAsync(clientX, clientY).Wait();
                
            }

        }

        private void BlazorLeafletOffline_ElementClicked(object sender, ElementClickedEventArgs e)
        {           
            Message += $"User Clicked Id {e.ElementId}  ";
            Console.WriteLine(Message);
            StateHasChanged();
        }
        public async Task ShowTiles()
        {
            await BlazorLeafletOffline.ShowTileList();
        }
        
        public async Task ShowMap()
        {
            var settings = new Settings()
            {
                AvailableLeafletTypes = new LeafletType[] { LeafletType.First, LeafletType.Second },
                LayerSwitcherVisible=true,
                OfflineButtonsVisible=false,
                DefaultLat = 49.225698949058142F,
                DefaultLong = -124.009291826127736F,
                MinZoom = 5,
                OfflineLayerAvailable=false,
                // not strictly needed with telerik context menu, but an easy way to get the coordinates
                ReplaceContextMenu=true // make sure to add css for disabling ios context long press (see app.css)
            };

            //string style1= "{ 'color': '#ff7800', 'weight': 5, 'opacity': 0.65}";
            string styleParks = "{ \"color\": \"#73b761\", \"opacity\": 0.65}";
            string styleTrails = "{ \"color\": \"#ECC846\", \"weight\": 5, \"opacity\": 1}";
            string styleBikeRoutes = "{ \"color\": \"#EE9E64\", \"weight\": 5, \"opacity\": 1}";


            //settings.DefaultSettings.DisplayMode = DisplayMode.Inline;
            await BlazorLeafletOffline.LogVar(settings);
            await BlazorLeafletOffline.InitializeMap("map", settings);
            //string geoJson1 =await Http.GetStringAsync("LandUseDistricts.geojson");
            string geoJson1 = await Http.GetStringAsync("NanaimoPARKS.geojson");
            string layerName = "Nanaimo Parks";
            //string geoJson1 = await Http.GetStringAsync("testGeoJson.json");
            await BlazorLeafletOffline.AddLayer(layerName, styleParks);
            await BlazorLeafletOffline.AddGeoJsonToLayer(layerName, geoJson1);

            string geoJson2 = await Http.GetStringAsync("Nanaimo_TRAILS.geojson");
            string layerName2 = "Nanaimo Trails";
            await BlazorLeafletOffline.AddLayer(layerName2, styleTrails);
            await BlazorLeafletOffline.AddGeoJsonToLayer(layerName2, geoJson2);

            string geoJson3 = await Http.GetStringAsync("Nanaimo_BIKE_ROUTES.geojson");
            string layerName3 = "Nanaimo Bike Routes";
            await BlazorLeafletOffline.AddLayer(layerName3, styleBikeRoutes);
            await BlazorLeafletOffline.AddGeoJsonToLayer(layerName3, geoJson3);

            await BlazorLeafletOffline.Show();

            for (markerCount = 0; markerCount < 5; markerCount++)
            {
                // TOODO: optimize
                string carsvg = $"<svg id='drive-{markerCount}' onclick='window.leafletElementClicked(\"drive-{markerCount}\");' xmlns='http://www.w3.org/2000/svg' style='position: fixed;right: 20px;' height='24px' viewBox='0 0 24 24' width='24px' fill='#000000'><path d='M0 0h24v24H0V0z' fill='none'/><path d='M18.92 6.01C18.72 5.42 18.16 5 17.5 5h-11c-.66 0-1.21.42-1.42 1.01L3 12v8c0 .55.45 1 1 1h1c.55 0 1-.45 1-1v-1h12v1c0 .55.45 1 1 1h1c.55 0 1-.45 1-1v-8l-2.08-5.99zM6.85 7h10.29l1.08 3.11H5.77L6.85 7zM19 17H5v-5h14v5z'/><circle cx='7.5' cy='14.5' r='1.5'/><circle cx='16.5' cy='14.5' r='1.5'/></svg>";
                string editsvg = $"<svg id='edit-{markerCount}' onclick='window.leafletElementClicked(\"edit-{markerCount}\");' xmlns='http://www.w3.org/2000/svg'  height='24px' viewBox='0 0 24 24' width='24px' fill='#000000'><path d='M0 0h24v24H0V0z' fill='none'/><path d='M14.06 9.02l.92.92L5.92 19H5v-.92l9.06-9.06M17.66 3c-.25 0-.51.1-.7.29l-1.83 1.83 3.75 3.75 1.83-1.83c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.2-.2-.45-.29-.71-.29zm-3.6 3.19L3 17.25V21h3.75L17.81 9.94l-3.75-3.75z'/></svg>";
                await BlazorLeafletOffline.AddMarker(new Marker()
                {
                    Lat = settings.DefaultLat.Value + (float)((random.NextDouble() -0.5) * 0.1),
                    Long = settings.DefaultLong.Value + (float)((random.NextDouble() -0.5) * 0.1),

                    PopupHtml = $"WO-123456<br/><hr/>{editsvg}{carsvg}"
                }
                );
            }

        }
    }
}
