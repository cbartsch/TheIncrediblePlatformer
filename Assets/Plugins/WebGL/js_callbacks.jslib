mergeInto(LibraryManager.library, {

  JS_LevelFinished: function (world, level) {
    console.log("JS level finished:", world, level)

    if(typeof gtag === "undefined") return;

   /*
    gtag("event", "LevelFinished", {
    	worldNum: world,
    	levelNum: level
    });
    */

    gtag("event", "LevelFinished_" + world + "_" + level);
  },
})