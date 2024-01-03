using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO3;
using BestHTTP.SocketIO3.Transports;

public static class Extension
{
    #region JaggedArr_Extensions
    public static void Add<T>(this T[][] jaggedArr, int offset, T value1, T value2, T value3)
    {
        if (offset > jaggedArr.Length - 1)
        {
            Debug.LogError("OutOfRange");
            return;
        }

        jaggedArr[offset] = new T[3];

        jaggedArr[offset][0] = value1;
        jaggedArr[offset][1] = value2;
        jaggedArr[offset][2] = value3;
    }

    public static T[][] Slice<T>(this T[][] jaggedArr, int range)
    {
        var slice = new T[range][];
        for (int i = 0; i < slice.Length; i++)
        {
            slice[i] = jaggedArr[i];
        }
        return slice;
    }

    //public static T[][] Slice<T>(this T[][] matrix, Range rows, Range columns)
    //{
    //    var slice = matrix[rows];
    //    for (int i = 0; i < slice.Length; i++)
    //    {
    //        slice[i] = slice[i][columns];
    //    }
    //    return slice;
    //}
    #endregion

    #region SocketIo_Extention
    public static void EmitCallBack<T>(this Socket socket, Action<T> CallBack, string eventName, params object[] args)
    {
        socket.ExpectAcknowledgement(CallBack).Emit(eventName, args);
    }
    #endregion
}

