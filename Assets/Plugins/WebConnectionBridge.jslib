var jslib = {
  $Data: {
    ORIGIN: [],
    onResponsePost : null,
  },

  

  initialize: function (origin) {
    if (UTF8ToString(origin) === "dev") {
      Data.ORIGIN = [
        "https://tournament.dev.playdapp.com",
        "http://localhost:3005",
        "http://localhost:3000",
        "http://172.16.32.11:3000"
      ];
    }
     else if (UTF8ToString(origin) === "qa") {
        Data.ORIGIN = [
          "https://tournament.qa.playdapp.com",
          "http://localhost:3005",
          "http://localhost:3000",
        ];
    }
     else if (UTF8ToString(origin) === "prod") {
        Data.ORIGIN = [
          "https://tournament.playdapp.com",
        ];
    }
    window.addEventListener("message", unityInstance.Module.WebEventListener);
  },
  
  SendFrontPostMessage: function(postMessage) {
       unityInstance.Module.SendPostMessage(UTF8ToString(postMessage));
       
       if(UTF8ToString(postMessage).includes("loading") || UTF8ToString(postMessage).includes("sendStartGame") || UTF8ToString(postMessage).includes("sendEndGame")) {
          
          if(UTF8ToString(postMessage).includes("pid")) return;
          
          var dataMessage = JSON.stringify({isSuccess : true});
             
          var encoder = new TextEncoder();
          var strBuffer = encoder.encode(dataMessage + String.fromCharCode(0));
          var strPtr = _malloc(strBuffer.length);
          HEAP8.set(strBuffer, strPtr);
          
          Module['dynCall_vi']( Data.onResponsePost, [strPtr]);		
          _free(strPtr);          
       }
    },
    
    
     PostOnMessage: function(callBack) {
       Data.onResponsePost = callBack;     
    },
};

autoAddDeps(jslib, "$Data");
mergeInto(LibraryManager.library, jslib);
