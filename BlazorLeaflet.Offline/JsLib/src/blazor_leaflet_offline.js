import LeafletOffline from 'leaflet.offline.tailslide';
import { getStorageInfo, getStoredTilesAsJson, saveTile } from 'leaflet.offline.tailslide';
import Pressure from 'pressure';
import $ from "jquery";

var ElementId = undefined;
var map = undefined;
let storageLayer;
let progress;
let baseLayer;
let layerswitcher;
let control;
let getGeoJsonData;
let addStorageLayer;
let myDotNetHelper;


// Used as a javascript callback on popup menu items (edit, drive buttons)

window.leafletElementClicked = (arg) => {
    myDotNetHelper.invokeMethodAsync('JSCallBackElementClicked',arg);
        //.then(message => console.log(message));
}

export function addMarker(dotNetHelper, marker) {
    L.marker([marker.lat, marker.long]).addTo(map)
        .bindPopup(marker.popupHtml);
}

let addedLayers = {};

function onEachFeature(feature, layer) {
    // does this feature have a property named popupContent?
    //if (feature.properties && feature.properties.popupContent) {
    //layer.bindPopup(feature.properties.popupContent);
    // display every property
    layer.bindPopup('<pre>' + JSON.stringify(feature.properties, null, ' ').replace(/[\{\}"]/g, '') + '</pre>');
    //}
}

export function addLayer(layerName, styleJson) {
    addedLayers[layerName] = L.geoJSON(false, { onEachFeature: onEachFeature, style: JSON.parse(styleJson)}).addTo(map);

    if (layerswitcher !== undefined)
        layerswitcher.addOverlay(addedLayers[layerName], layerName);
}




export function addGeoJsonToLayer(layerName, geojsonFeature) {
    addedLayers[layerName].addData(JSON.parse(geojsonFeature));
}

//$.pressureConfig({});
let lat = 0;
let lng = 0; // last clicked coords
function openContextMenu(e, touchPress) {
    //console.log("Firing open context menu");
    //console.log(e);
    //var latlng = event.latlng; // L.mouseEventToLatLng(event.originalEvent);

    //const containerPoint = L.mouseEventToContainerPoint(event.originalEvent);
    //const layerPoint = L.containerPointToLayerPoint(containerPoint);
    //const latlng = L.layerPointToLatLng(layerPoint);
    let event = e;
    if (event.clientX === undefined) event = e.originalEvent; 

    const bubbledEvent = `{ "touchPress" : ${touchPress}, "lat": ${lat}, "lng": ${lng},  "clientX": ${event.clientX}, "clientY": ${event.clientY}, "offsetX": ${event.offsetX}, "offsetY": ${event.offsetY}, "pageX": ${event.pageX}, "pageY": ${event.pageY}, "screenX": ${event.screenX}, "screenY": ${event.screenY} }`;
    console.log(bubbledEvent);

    myDotNetHelper.invokeMethodAsync('JSCallBackOpenContextMenu', bubbledEvent);
}

function replaceContextMenu(elementId, map) {
    const element = '#' + elementId;
    var block= {
        startDeepPress: function (event) {
            // this is called on "force click" / "deep press", aka once the force is greater than 0.5
            openContextMenu(event, true);
        },
    //    endDeepPress: function () {
    //        console.log('EndDp');
    //    },
    //    start: function (event) {
    //        // this is called on force start
    //        console.log('Pressurestart');
    //    },
    //    end: function () {
    //        // this is called on force end
    //        console.log('PressureEnd');
    //    },
    //    change: function (force, event) {
    //        // this is called every time there is a change in pressure
    //        // force will always be a value from 0 to 1 on mobile and desktop
    //        console.log('Change');
    //    },
    //    unsupported: function () {
    //        // NOTE: this is only called if the polyfill option is disabled!
    //        // this is called once there is a touch on the element and the device or browser does not support Force or 3D touch
    //        console.log('Unsupported');
    //    }
    };
    Pressure.set(element, block);
    //map = L.map(elementId);
    map.on("contextmenu", function (event) {
        //console.log("Coordinates: " + event.latlng.toString());
        //L.marker(event.latlng).addTo(map);
        lat = event.latlng.lat;
        lng = event.latlng.lng;
        openContextMenu(event, false);
        return false;
    });
    map.on('click', function (event) {
        lat = event.latlng.lat;
        lng = event.latlng.lng;
    });
    //$(element).bind('contextmenu', function (e) {
    //    openContextMenu(e);
    //    return false;
    //});
}

export function initializeMap(dotNetHelper, elementId, settings) {


    ElementId = elementId;
    myDotNetHelper = dotNetHelper;
    console.log("element");
    console.log(elementId);
    console.log("settings");
    console.log(settings);
    map = L.map(elementId);
    // offline baselayer, will use offline source if available
    let baseLayer = L.tileLayer
        .offline(urlTemplate, {
            attribution: settings.attribution,
            subdomains: settings.subdomains,
            minZoom: settings.minZoom,
            saveOnLoad: settings.saveOnLoad,
            downsample: settings.downsample
        })
        .addTo(map);


    //$('#storageModal').on('show.bs.modal', () => {
    //    alert("hi");
    //    console.log("showing tile list");
    //    getStorageInfo(urlTemplate).then((r) => {
    //        const list = document.getElementById('tileinforows');
    //        console.log(r);
    //        list.innerHTML = '';
    //        for (let i = 0; i < r.length; i += 1) {
    //            const createdAt = new Date(r[i].createdAt);
    //            list.insertAdjacentHTML(
    //                'beforeend',
    //                `<tr><td>${i}</td><td>${r[i].url}</td><td>${r[i].key
    //                }</td><td>${createdAt.toDateString()}</td></tr>`,
    //            );
    //        }
    //    });
    //});
    map.setView(
        {
            lat: settings.defaultLat,
            lng: settings.defaultLong,
        },
        settings.defaultZoom,
    );

    // layer switcher control
    if (settings.layerSwitcherVisible) {
        layerswitcher = L.control
            .layers({
                'osm (offline)': baseLayer,
            }, null, { collapsed: false })
            .addTo(map);
        if (settings.offlineLayerAvailable) {
            getGeoJsonData = () => getStorageInfo(urlTemplate)
                .then((data) => getStoredTilesAsJson(baseLayer, data));
            addStorageLayer = () => {
                getGeoJsonData().then((geojson) => {
                    storageLayer = L.geoJSON(geojson).bindPopup(
                        (clickedLayer) => clickedLayer.feature.properties.key,
                    );
                    layerswitcher.addOverlay(storageLayer, 'stored tiles');
                });
            };
            addStorageLayer();
        }
    }

    if (settings.offlineButtonsVisible) {
        // add buttons to save tiles in area viewed
        let control = L.control.savetiles(baseLayer, {
            zoomlevels: [13, 16], // optional zoomlevels to save, default current zoomlevel
            confirm(layer, successCallback) {
                // eslint-disable-next-line no-alert
                if (window.confirm(`Save ${layer._tilesforSave.length}`)) {
                    successCallback();
                }
            },
            confirmRemoval(layer, successCallback) {
                // eslint-disable-next-line no-alert
                if (window.confirm('Remove all the tiles?')) {
                    successCallback();
                }
            },
            saveText:
                '<i class="fa fa-download" aria-hidden="true" title="Save tiles"></i>',
            rmText: '<i class="fa fa-trash" aria-hidden="true"  title="Remove tiles"></i>',
        });
        control.addTo(map);

        document.getElementById('remove_tiles').addEventListener('click', () => {
            control._rmTiles();
        });
    }

    baseLayer.on('storagesize', (e) => {
        document.getElementById('storage').innerHTML = e.storagesize;
        if (storageLayer) {
            storageLayer.clearLayers();
            getGeoJsonData().then((data) => {
                storageLayer.addData(data);
            });
        }
    });
    // events while saving a tile layer
    baseLayer.on('savestart', (e) => {
        progress = 0;
        document.getElementById('total').innerHTML = e._tilesforSave.length;
    });
    baseLayer.on('savetileend', () => {
        progress += 1;
        document.getElementById('progress').innerHTML = progress;
    });

    if (settings.replaceContextMenu) replaceContextMenu(elementId, map);

}

export function show() {
}

const urlTemplate = 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';

export function showTileList() {
    alert("hi");
    console.log("showing tile list");
    getStorageInfo(urlTemplate).then((r) => {
        const list = document.getElementById('tileinforows');
        console.log(r);
        list.innerHTML = '';
        for (let i = 0; i < r.length; i += 1) {
            const createdAt = new Date(r[i].createdAt);
            list.insertAdjacentHTML(
                'beforeend',
                `<tr><td>${i}</td><td>${r[i].url}</td><td>${r[i].key
                }</td><td>${createdAt.toDateString()}</td></tr>`,
            );
        }
    });
}




