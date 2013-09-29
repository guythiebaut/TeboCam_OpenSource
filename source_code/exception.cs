using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace TeboCam
{
    class exception
    {

        public void newException(Exception ex)
        {

     
            

            exeptionInstance extmp = new exeptionInstance();
            extmp.hResult= Marshal.GetHRForException( ex );


        }





        public class exeptionInstance
        {

            public int hResult;
            public string exceptionMessage;
            public string exception;

        }





    }
}
