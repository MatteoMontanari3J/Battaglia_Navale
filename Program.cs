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
                    -stampa tabella giocatore
                    -stampa tabella IA nascosta
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

                        -stampa tabella giocatore
                        -stampa tabella IA nascosta

                        -controllo fine partita
                            si: -finepartita
                            no: ricomincia il turno
            )
            #########################################################################################################                        
            */

            //dichiarazione ed inizializzazione delle variabili:

            //matrici del campo di gioco - 11 caselle perchè: 1.è più facile interragire con le coordinate da parte del giocatore 2.si usano come spazio per le lettere ad inizio codice
            //dichiarazione ed inizializzazione delle variabili:

            //matrici del campo di gioco
            char[,] player = new char[11, 11];
            // int[,] playerHidden = new int[11, 11];
            char[,] ia = new char[11, 11];
            // int[,] iaHidden = new int[11, 11];

            //variabili coordinate per la selezione della casella durante il gioco
            char riga = ' ';
            int colonna = 0;

            //variabile turno - cambia a seconda di chi è il turno (true è il campo del giocatore)
            bool turn = true;

            //variabile difficoltà
            bool difficoltà = false;

            //variabile per i risultati di alcune funzion
            string stato = "";



            TestPlayerFieldGeneration(player); // Player field
            Console.WriteLine("\n");
            TestBotFieldGeneration(ia); // Bot field

            SelezioneDifficoltà(ref difficoltà);
            if (difficoltà)
                Console.WriteLine("true");

            //fin
            Console.ReadKey();
        }

        /// <summary>
        /// funzione che permette al giocatore di selezionare la difficoltà
        /// </summary>
        /// <param name="difficoltà">variabile boolean che corrisponde alla difficoltà selezionata.</param>
        static void SelezioneDifficoltà(ref bool difficoltà)
        {
            //dichiarazione ed inizializzazione variabili
            char var = ' ';
            int i = 0;

            //ciclo per selezionare la difficoltà
            while (i == 0)
            {
                //assegnazione valori
                Console.WriteLine("\n\nScegliere la difficoltà:\nPremere 0 per difficoltà facile\nPremere 1 per difficoltà difficile.");
                var = Convert.ToChar(Console.ReadLine());
                //assegnazione della difficoltà selezionata
                switch (var)
                {
                    case '0':
                        difficoltà = false;
                        i++;
                        break;
                    case '1':
                        difficoltà = true;
                        i++;
                        break;
                    default:
                        Console.WriteLine("Perfavore, inserire una difficoltà esistente");
                        break;
                }

                //pulizia console di output
                Console.Clear();
            }
        }

        static void TestPlayerFieldGeneration(char[,] player) // FUNZIONE DI debug field
        {
            int let = 65; // ascii indice della lettera 'A'
            int numero = 49; // ascii indice di '1'

            Console.WriteLine("Campo del giocatore");
            for (int i = 0; i < player.GetLength(0); i++)
            {
                Console.WriteLine(); // obbligatorio perche senza questo riesce male
                for (int j = 0; j < player.GetLength(0); j++)
                {
                    /* 
                    j = riga (orizontale)
                    i = colonna (verticale)
                    */

                    if (j == 0 && i == 0)
                    {
                        Console.Write(' ' + " "); // nella posizione 0,0 lasciamo vuoto
                    }
                    else if (j != 0 && i == 0) // generazione delle lettere
                    {
                        char lettera = (char)(let); // trasforma ascii indice nel carattere
                        player[i, j] = lettera;
                        Console.Write(player[i, j] + " "); // stampa la lettera
                        let++; // ascii indice + 1 (trasforma nella lettera prossima)
                    }
                    else if (j == 0 && i != 0) // generazione dei numeri di posizione
                    {
                        if (i == 10) // se posizione = 10
                        {
                            numero = 48; // nella posizione di 10 mettiamo '0' (ascii indice di '0')
                        }

                        char num = (char)(numero); // trasforma ascii indice nel carattere

                        player[i, j] = num;
                        Console.Write(player[i, j] + " ");
                        numero++; // ascii indice + 1 (trasforma nel numero prossimo)

                    }
                    else // genera la cella "vuota" dove si puo giocare
                    {
                        player[i, j] = '?';
                        Console.Write(player[i, j] + " ");
                    }
                }
            }
        }


                                            //qui non dovrebbe essere (char [,] player) ?
        static void TestBotFieldGeneration(char[,] player) // FUNZIONE DI debug field
        {
            int let = 65; // ascii indice della lettera 'A'
            int numero = 49; // ascii indice di '1'

            Console.WriteLine("Campo del Bot");
            for (int i = 0; i < player.GetLength(0); i++)
            {
                Console.WriteLine(); // obbligatorio perche senza questo riesce male
                for (int j = 0; j < player.GetLength(0); j++)
                {
                    /* 
                    j = riga (orizontale)
                    i = colonna (verticale)
                    */

                    if (j == 0 && i == 0)
                    {
                        Console.Write(' ' + " "); // nella posizione 0,0 lasciamo vuoto
                    }
                    else if (j != 0 && i == 0) // generazione delle lettere
                    {
                        char lettera = (char)(let); // trasforma ascii indice nel carattere
                        player[i, j] = lettera;
                        Console.Write(player[i, j] + " "); // stampa la lettera
                        let++; // ascii indice + 1 (trasforma nella lettera prossima)
                    }
                    else if (j == 0 && i != 0) // generazione dei numeri di posizione
                    {
                        if (i == 10) // se posizione = 10
                        {
                            numero = 48; // nella posizione di 10 mettiamo '0' (ascii indice di '0')
                        }

                        char num = (char)(numero); // trasforma ascii indice nel carattere

                        player[i, j] = num;
                        Console.Write(player[i, j] + " ");
                        numero++; // ascii indice + 1 (trasforma nel numero prossimo)

                    }
                    else // genera la cella "vuota" dove si puo giocare
                    {
                        player[i, j] = '?';
                        Console.Write(player[i, j] + " ");
                    }
                }
            }
        }
    }
}





//il codice per la schermata di titolo sta nel drive di Teo