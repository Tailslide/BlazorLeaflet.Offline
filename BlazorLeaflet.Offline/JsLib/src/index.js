import { initializeMap, show, showTileList, addMarker, addLayer, addGeoJsonToLayer } from './blazor_leaflet_offline';

export function InitializeMap(helper, elementId, settings) {
    //console.log("belement");
    //console.log(elementId);
    //console.log("bsettings");
    //console.log(settings);
    //console.log("btest");
    //console.log(test);
    return initializeMap(helper, elementId, settings);
}

export function AddLayer(layername, styleJson) {
    return addLayer(layername, styleJson);
}

export function AddGeoJsonToLayer(layerName, geojsonFeature) {
    return addGeoJsonToLayer(layerName, geojsonFeature);
}


export function LogVar(obj) {
    console.log("Debugging value:");
    console.log(obj);
}

export function AddMarker(helper, marker) {
    return addMarker(helper, marker);
}

export function Show() {
    return show();
}

export function ShowTileList() {
    return showTileList();
}

