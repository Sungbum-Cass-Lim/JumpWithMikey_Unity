Module.WebEventListener = function (e) {
  
  if (Data.ORIGIN.indexOf(e.origin) === -1 && Data.ORIGIN.indexOf("") === -1) {
  return;
  }

  let message = null;
    
  // json 형식이 아니면 안됨
  try {
    message = JSON.parse(e.data);
   
  } catch (error) {
    return;
  }
    console.log('[message] :: ', message);
    if(Data.onResponsePost === null || Data.onResponsePost === undefined) return;
     var dataMessage;
    if(message.message.includes("onRestart")) {
      dataMessage = JSON.stringify({onRestart : true});
    }
    else if(message.message.includes("onStartGame")) {
      dataMessage = JSON.stringify({isSuccess : true});
    }
    else{
      dataMessage = JSON.stringify(message.data)
    }

    var encoder = new TextEncoder();
    var strBuffer = encoder.encode(dataMessage + String.fromCharCode(0));
    var strPtr = _malloc(strBuffer.length);
    HEAP8.set(strBuffer, strPtr);
    
    Module['dynCall_vi']( Data.onResponsePost, [strPtr]);		
    _free(strPtr);
};

Module.SendPostMessage = function (data) {
  window.parent.postMessage(data, "*");
};

// postMessage object return
Module.GetPostMessage = function (message, data) {
  if (data === null || data === undefined || data === "") data = {};
  return { message: message, data: data };
};
