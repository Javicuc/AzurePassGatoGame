using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace AppGame
{
    public class FragmentTablero : Fragment
    {
        public string[] tablero { get; set; }
        public List<Button> btTablero { get; set; }
        public int wins1 { get; set; }
        public int wins2 { get; set; }
        public int jugadaActual { get; set; }
        public Boolean turnoActual { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }
    }
}