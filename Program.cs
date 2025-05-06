using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battaglia_Navale
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            ######################################################################################################### 
            STRUTTURA DEL MAIN   #  i nomi con il - affianco sono funzioni
            ######################################################################################################### 
            ripetizione
            (
                (-selezione difficoltà
                 -assegnamento barche giocatore
                 -assegnamento barche IA
                 azzeramento booleano che determina l'inizio partita)
                
                -turno giocatore
                    prendere coordinate
                    -funzione sparare il colpo e vedere se si ha beccato o mancato

                -controllo fine partita
                    si: -finepartita
                    no:(
                        a seconda della difficoltà selezionata
                            facile
                                -turno IA random
                                    generare coordinate random
                            difficile
                                -turno IA smart
                                    [algoritmo intelligenza] ottenere coordinate

                        -funzione sparare il colpo e vedere se si ha beccato o mancato

                        -controllo fine partita
                            si: -finepartita
                            no: ricomincia il turno
            )
            #########################################################################################################                        
            */

            //dichiarazione ed inizializzazione delle variabili:

            //matrici del campo di gioco
            int[,] player = new int[10, 10];
            int[,] playerHidden = new int[10, 10];
            int[,] ia = new int[10, 10];
            int[,] iaHidden = new int[10, 10];

            

            //variabili coordinate per la selezione della casella durante il gioco
            char riga = ' ';
            int colonna = 0;

            //variabile turno - cambia a seconda di chi è il turno
            char turn = ' ';


            



            //fin
            Console.ReadKey();
        }
    }
}
