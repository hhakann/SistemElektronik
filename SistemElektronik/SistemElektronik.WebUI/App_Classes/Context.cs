using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SistemElektronik.WebUI.Models;

namespace SistemElektronik.WebUI.App_Classes
{
    
    public class Context
    {
        private static E_TicaretEntities baglanti;
        public static E_TicaretEntities Baglanti
        {
            get {
                if (baglanti == null) baglanti = new E_TicaretEntities();
                    return baglanti; }
            set { baglanti = value; }
        }

      
    
    }
}