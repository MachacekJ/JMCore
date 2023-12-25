// // https://swharden.com/blog/2022-05-29-blazor-loading-progress/
// //https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/startup?view=aspnetcore-6.0
// function StartBlazor() {
//     const progressbar = document.getElementById('progress-bar');
//     const progressLabel = document.getElementById('progressLabel');
//     let loadedCount = 0;
//     const resourcesToLoad = [];
//     Blazor.start({
//         loadBootResource:
//             function (type, filename, defaultUri, integrity) {
//                 console.log(`Loading: '${type}', '${name}', '${defaultUri}', '${integrity}'`);
//                 if (type == "dotnetjs")
//                     return defaultUri;
//
//                 const fetchResources = fetch(defaultUri, {
//                     cache: 'no-cache',
//                     integrity: integrity,
//                     headers: { 'MyCustomHeader': 'My custom value' }
//                 });
//
//                 resourcesToLoad.push(fetchResources);
//
//                 fetchResources.then((r) => {
//                     console.log(`Loading: '${type}', '${name}', '${defaultUri}', '${integrity}'`);
//                     loadedCount += 1;
//                     if (filename == "blazor.boot.json") {
//                         return;
//                     }
//                     const totalCount = resourcesToLoad.length;
//                     const percentLoaded = 10 + parseInt((loadedCount * 90.0) / totalCount);
//                     //const progressbar = document.getElementById('progressbar');
//                     progressbar.style.width = percentLoaded + '%';
//                     progressbar.innerText = percentLoaded + '%';
//                     //const progressLabel = document.getElementById('progressLabel');
//                     progressLabel.innerText = `Downloading ${loadedCount}/${totalCount}: ${filename}`;
//                 });
//
//                 return fetchResources;
//             }
//     });
// }
// document.addEventListener("DOMContentLoaded", function () {
//    StartBlazor();
// });