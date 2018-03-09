var kongregate;

mergeInto(LibraryManager.library, {
  
  JS_Init: function() {
    if(kongregateAPI) {
      console.log("loading kongregate API");
      kongregateAPI.loadAPI(function() {
        console.log("loaded kongregate API");
        kongregate = kongregateAPI.getAPI();
      });
    } else {
      console.log("no kongregate API available");
    }

    if(typeof gtag !== "undefined") {
      gtag("event", "StartGame");
    }
  },

  JS_Destroy: function() {

  },

  JS_LevelFinished: function (world, level) {
    console.log("JS level finished:", world, level);
    if(typeof gtag !== "undefined") {
      gtag("event", "LevelFinished_" + world + "_" + level);
    }

    if(kongregate) {
      kongregate.stats.submit("LevelFinished", (world * 1000) + level);
    }
  },

  JS_GameFinished: function () {
    console.log("JS game finished");

    if(typeof gtag !== "undefined") {
      gtag("event", "GameFinished");
    }

    if(kongregate) {
      kongregate.stats.submit("GameFinished", 1);
    }
  },
})