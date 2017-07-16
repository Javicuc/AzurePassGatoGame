using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace AppGame
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        Button btFila1A, btFila1B, btFila1C;
        Button btFila2A, btFila2B, btFila2C;
        Button btFila3A, btFila3B, btFila3C;
        Button btReiniciar; // Boton que reiniciara el juego
        Button[] btTablero; // Almacenara los botones del tablero (Grid Layout)
        TextView tvJugador1Wins, tvJugador2Wins, tvTurnoJugador;

        string[] Tablero;      // Almacenara X u O en la posición solicitada
        string jugadorGanador; // Variable que determinara que jugador gana
        bool turno;            // Variable que determinara el turno de los jugadores
        int jugador1Wins = 0;  // Victorias jugador 1
        int jugador2Wins = 0;  // Victorias jugador 2
        int jugadas;           // Cantidad de jugadas realizadas (Determina empates)

        FragmentTablero tableroPersistencia;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Tablero);
            

            tvJugador1Wins = FindViewById<TextView>(Resource.Id.tvWinsJugador1);
            tvJugador2Wins = FindViewById<TextView>(Resource.Id.tvWinsJugador2);
            tvTurnoJugador = FindViewById<TextView>(Resource.Id.tvTurnoJugador);  
            btReiniciar = FindViewById<Button>(Resource.Id.btReiniciar);
            //btReiniciar.SetBackgroundColor(Android.Graphics.Color.Sienna);

            btFila1A = FindViewById<Button>(Resource.Id.buttonF1A);
            btFila1B = FindViewById<Button>(Resource.Id.buttonF1B);
            btFila1C = FindViewById<Button>(Resource.Id.buttonF1C);

            btFila2A = FindViewById<Button>(Resource.Id.buttonF2A);
            btFila2B = FindViewById<Button>(Resource.Id.buttonF2B);
            btFila2C = FindViewById<Button>(Resource.Id.buttonF2C);

            btFila3A = FindViewById<Button>(Resource.Id.buttonF3A);
            btFila3B = FindViewById<Button>(Resource.Id.buttonF3B);
            btFila3C = FindViewById<Button>(Resource.Id.buttonF3C);


            // Fragment que persistira los datos del juego
            tableroPersistencia = (FragmentTablero)this.FragmentManager.FindFragmentByTag("tablero");
            if(tableroPersistencia == null)
            {
                var FragmentTransaction = this.FragmentManager.BeginTransaction();
                tableroPersistencia = new FragmentTablero();

                initGame(); // Inicializamos el juego

                tableroPersistencia.btTablero = btTablero;
                tableroPersistencia.tablero = Tablero;
                tableroPersistencia.winsJugador1 = jugador1Wins;
                tableroPersistencia.winsJugador2 = jugador2Wins;
                tableroPersistencia.jugadas = jugadas;
                tableroPersistencia.turnoActual = turno;


                FragmentTransaction.Add(tableroPersistencia, "tablero");
                FragmentTransaction.Commit();
            }
            else
            {
                btTablero = tableroPersistencia.btTablero;
                Tablero = tableroPersistencia.tablero;
                jugador1Wins = tableroPersistencia.winsJugador1;
                jugador2Wins = tableroPersistencia.winsJugador2;
                jugadas = tableroPersistencia.jugadas;
                turno = tableroPersistencia.turnoActual;
            }

            foreach (var x in btTablero)
            {
                x.Click += (s, e) =>
                {
                    clickTablero(x);
                    jugadas++;

                    if (verificarGanador())
                    {
                        showAlert("Hay un Ganador",
                            "La partida ha finalizado, ¿Desea reiniciar el encuentro?");

                        incrementarPuntos();
                    }
                    else if (jugadas == 9)
                    {
                        showAlert("Hay un empate!",
                            "¿Desea reiniciar el encuentro?");
                    }

                    tvTurnoJugador.Text = turno ? "1" : "2";
                };
            }

            btReiniciar.Click += (sen, eve) =>
            {
                initGame();
            };

        }

        private void clickTablero(Button x)
        {
            // Depende del turno del jugador, se setea X u O en el boton clickeado
            if (turno)
            {
                x.Text = "O";
                x.SetTextColor(Android.Graphics.Color.Orange);
            }
            else
            {
                x.Text = "X";
                x.SetTextColor(Android.Graphics.Color.Green);
            }

            // Depende del botón clickeado, por medio de su ID, 
            // almacenaremos el texto que tiene seteado.
            switch (x.Id)
            {
                case Resource.Id.buttonF1A:
                    Tablero[0] = x.Text;
                    break;
                case Resource.Id.buttonF1B:
                    Tablero[1] = x.Text;
                    break;
                case Resource.Id.buttonF1C:
                    Tablero[2] = x.Text;
                    break;
                case Resource.Id.buttonF2A:
                    Tablero[3] = x.Text;
                    break;
                case Resource.Id.buttonF2B:
                    Tablero[4] = x.Text;
                    break;
                case Resource.Id.buttonF2C:
                    Tablero[5] = x.Text;
                    break;
                case Resource.Id.buttonF3A:
                    Tablero[6] = x.Text;
                    break;
                case Resource.Id.buttonF3B:
                    Tablero[7] = x.Text;
                    break;
                case Resource.Id.buttonF3C:
                    Tablero[8] = x.Text;
                    break;
            }

            x.Clickable = false; // Bloqueamos el boton que ha sido clickeado

            turno = !turno; // Cambiamos el turno del jugador
            
        }

        public void initGame()
        {
            Tablero = new string[9] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            jugadorGanador = "K";
            jugadas = 0;
            turno = true;


            // Array que contiene todos los botones del tablero
            btTablero = new Button[] {btFila1A,btFila1B,btFila1C,
                                      btFila2A,btFila2B,btFila2C,
                                      btFila3A,btFila3B,btFila3C};

            tvTurnoJugador.Text = turno ? "1" : "2";

            foreach (var x in btTablero)
            {
                x.Clickable = true;
                x.Text = GetString(Resource.String.clickeame);
                x.SetBackgroundResource(Resource.Drawable.buttonStyleApp);
                x.SetTextColor(Android.Graphics.Color.White);
            }
        }
        public bool verificarGanador()
        {
            //Horizontales
            if (Tablero[0] == Tablero[1] && Tablero[1] == Tablero[2])
            {
                jugadorGanador = Tablero[0];
                return true;

            }
            else if (Tablero[3] == Tablero[4] && Tablero[4] == Tablero[5])
            {
                jugadorGanador = Tablero[3];
                return true;
            }
            else if (Tablero[6] == Tablero[7] && Tablero[7] == Tablero[8])
            {
                jugadorGanador = Tablero[6];
                return true;
            }
            //Verticales
            else if (Tablero[0] == Tablero[3] && Tablero[3] == Tablero[6])
            {
                jugadorGanador = Tablero[0];
                return true;
            }
            else if (Tablero[1] == Tablero[4] && Tablero[4] == Tablero[7])
            {
                jugadorGanador = Tablero[1];
                return true;
            }
            else if (Tablero[2] == Tablero[5] && Tablero[5] == Tablero[8])
            {
                jugadorGanador = Tablero[2];
                return true;
            }
            //Diagonales
            else if (Tablero[0] == Tablero[4] && Tablero[4] == Tablero[8])
            {
                jugadorGanador = Tablero[0];
                return true;
            }
            else if (Tablero[2] == Tablero[4] && Tablero[4] == Tablero[6])
            {
                jugadorGanador = Tablero[2];
                return true;
            }
            else
            {
                return false;
            }
        }

        public void incrementarPuntos()
        {

            if (jugadorGanador.Equals("O"))
            {
                jugador1Wins++;
                tvJugador1Wins.Text = jugador1Wins.ToString();
            }
            else if (jugadorGanador.Equals("X"))
            {
                jugador2Wins++;
                tvJugador2Wins.Text = jugador2Wins.ToString();
            }
            
        }

        public void showAlert(string tittle, string message)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle(tittle);
            alert.SetMessage(message);
            alert.SetPositiveButton("Reiniciar", (se, ev) => {
                Toast.MakeText(this, "Inicia la partida!", ToastLength.Short).Show();
                initGame();
            });

            foreach (var x in btTablero)
                if (x.Text.Equals(GetString(Resource.String.clickeame)))
                {
                    x.SetBackgroundColor(Android.Graphics.Color.Red);
                    x.Clickable = false;
                    x.Text = "Fin";
                }

            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}

