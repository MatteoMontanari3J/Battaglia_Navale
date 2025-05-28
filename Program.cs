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
                                    [algoritmo intelligente] ottenere coordinate

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

            //matrici del campo di gioco - 11 caselle perchè: 1.è più facile interragire con le coordinate da parte del giocatore 2.si usano come spazio per le lettere ad inizio
            //codice dichiarazione ed inizializzazione delle variabili:

            //matrici del campo di gioco
            char[,] player = new char[11, 11];
            char[,] playerHidden = new char[11, 11];
            char[,] ia = new char[11, 11];
            char[,] iaHidden = new char[11, 11];

            //variabili coordinate per la selezione della casella durante il gioco
            int riga = 0;
            char colonna = ' ';
            //altre variabili dentro il ciclo
            bool turn = true;           //variabile turno - cambia a seconda di chi è il turno (true è il giocatore - false è l'IA)
            bool difficoltà = false;    //variabile difficoltà
            bool colpito = false;

            //altre variabili fuori dal ciclo
            bool gameRepeat = true;     //variabile per la ripetizione del gioco
            bool gameStart = true;      //variabile che dice se la partita è appena iniziata o no. utilizzata per la selezione della difficoltà e per il posizionamento delle barche


            //punti vita delle barche per il colpito e affondato
            int[,] boatsPlayer = new int[10, 4];                      //10 barche totali ciascuno
            int[,] boatsIa = new int[10, 4];
            //l'ordine è: coordriga, coordcolonna, vert/orizz, lunghezza.
            //la prima coordinata serve per l'identificativo barca

            bool end = false;   //variabile per determinare se la partita è finita o no

            //-----------FINE INIZIALIZZAZIONE-----------

            SchermataIniziale();    //visualizzazione della schermata di titolo

            //ciclo partita
            while (gameRepeat)
            {
                if (gameStart)   //inizio partita
                {
                    turn = true;        //re-inizializzazione in caso di una nuova partita
                    gameStart = true;
                    riga = 0;
                    colonna = ' ';
                    colpito = false;
                    end = false;

                    for (int i = 0; i < 10; i++)    //setta a 0 tutte le righe
                    {
                        SetZero(boatsPlayer, i);
                        SetZero(boatsIa, i);
                    }

                    FieldGeneration(player);    //riempimento dei campi di gioco, giocatore, IA e quelli nascosti
                    FieldGeneration(ia);
                    FieldGeneration(iaHidden);
                    FieldGeneration(playerHidden);

                    //fine re-inizializzazione

                    SelezioneDifficoltà(ref difficoltà);    //selezione della difficoltà

                    ShipPlacement(player, riga, colonna, boatsPlayer);   //posizionamento barche del giocatore

                    ShipPlacementIA(ia, boatsIa);    //posizionemento barche dell'IA

                    gameStart = false;  //la sequenza di inizio partita è finita, quindi non deve essere ripetuta nel prossimo ciclo
                }

                TurnoGiocatore(ia, player, iaHidden, playerHidden, ref colpito, ref turn, ref riga, ref colonna, boatsIa);  //turno del giocatore

                end = VerificaFine(player, ia, ref gameRepeat, ref gameStart);   //controlla se la parita è finita

                if (!end)    //se la partita non è finita
                {
                    if (difficoltà) //se la difficoltà è alta
                    {
                        TurnoIaSmart(ia, player, iaHidden, playerHidden, ref colpito, ref turn, ref riga, ref colonna, boatsPlayer);
                    }
                    else    //se la difficoltà è bassa
                    {
                        TurnoIARandom(ia, player, iaHidden, playerHidden, ref colpito, ref turn, ref riga, ref colonna, boatsPlayer);
                    }

                    VerificaFine(player, ia, ref gameRepeat, ref gameStart);    //verifica di nuovo se la partita è finita
                }
            }

            //fin
            Console.WriteLine("\n\n\n\n\nPremere qualsiasi tasto per uscire dal programma...");
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

        /// <summary>
        /// funzione di riempimento delle matrici di gioco
        /// </summary>
        /// <param name="player"> matrice da riempire </param>
        static void FieldGeneration(char[,] player) // Funzione di riempimento del campo
        {
            int let = 65; // indice ascii della lettera 'A'
            int numero = 49; // indice ascii di '1'


            for (int i = 0; i < player.GetLength(0); i++)
            {

                for (int j = 0; j < player.GetLength(0); j++)
                {
                    /* 
                    j = riga (orizontale)
                    i = colonna (verticale)
                    */

                    if (j == 0 && i == 0)
                    {
                        player[j, i] = ' ';
                    }
                    else if (j != 0 && i == 0) // generazione delle lettere
                    {
                        char lettera = (char)(let); // trasforma indice ascii nel carattere
                        player[i, j] = lettera;

                        let++; // ascii indice + 1 (trasforma nella prossima lettera)
                    }
                    else if (j == 0 && i != 0) // generazione dei numeri della posizione
                    {
                        if (i == 10) // se posizione = 10
                        {
                            numero = 48; // nella posizione di 10 mettiamo '0' (indice ascii di '0')
                        }

                        char num = (char)(numero); // trasforma ascii indice nel carattere

                        player[i, j] = num;

                        numero++; // indice ascii + 1 (trasforma nel prossimo numero)

                    }
                    else // genera la cella "vuota"
                    {
                        player[i, j] = '?';

                    }
                }
            }
        }

        /// <summary>
        /// funzione di piazzamento delle barche del giocatore
        /// </summary>
        /// <param name="player"> matrice in cui piazzare le barche </param>
        /// <param name="riga"></param>
        /// <param name="colonna"></param>
        static void ShipPlacement(char[,] player, int riga, char colonna, int[,] boats)
        {
            int ship1 = 4; //quantita dei navi (ship"1" - 1 -> lunghezza)
            int ship2 = 3;
            int ship3 = 2;
            int ship4 = 1;
            int somma = ship1 + ship2 + ship3 + ship4; // somma
            int scelta = 0;
            int colonnaCoord = 0;
            bool vert = false;
            bool passed = false;

            for (int i = 0; i < somma; i++)
            {

                Console.Clear();
                FieldShow(player);      //si mostra il campo del giocatore al giocatore

                Console.WriteLine('\n');
                Console.WriteLine("BARCHE DA PIAZZARE:");
                Console.WriteLine("Navi da 1 casella: " + ship1);
                Console.WriteLine("Navi da 2 caselle: " + ship2);
                Console.WriteLine("Navi da 3 caselle: " + ship3);
                Console.WriteLine("Navi da 4 caselle: " + ship4);
                Console.WriteLine("\n");
                Console.WriteLine("Scegli il tipo di nave da piazzare. Se provi a mettere una barca sopra l'altra, non succederà niente e la barca non verrà piazzata.");
                scelta = Convert.ToInt32(Console.ReadLine());

                boats[i, 1] = Convert.ToInt32(colonna);     //memorizzare la colonna

                if (scelta < 1 || scelta > 4)
                {
                    i--;
                    Console.WriteLine("Il tipo di nave selezionato non esiste.");
                }
                else if (scelta == 1 && ship1 != 0)
                {
                    boats[i, 3] = scelta;   //MEMORIZZAZIONE LUNGHEZZA

                    if (ship1 == 0)
                    {
                        Console.WriteLine("Non sono rimaste navi di questo tipo.");             //forse il bug del negativo sta qui in giro
                        i--;
                    }
                    else
                    {
                        passed = false; // per rinnovare il ciclo dopo la prima volta
                        while (!passed)
                        {
                            Console.WriteLine("Scegliere le coordinate della barca. Inserire 10 per la coordinata 0.");
                            Console.WriteLine("Coordinata della riga (numero):");
                            riga = int.Parse(Console.ReadLine());
                            Console.WriteLine("Coordinata della colonna (lettera)");
                            colonna = char.Parse(Console.ReadLine());

                            //boats[i, 1] = Convert.ToInt32(colonna);     //memorizzare la colonna old

                            ControlBarriers(riga, colonna, scelta, vert, ship1, ship2, ship3, ship4, ref passed);
                        }
                        if (AreaLibera(player, riga, colonna, colonnaCoord, scelta, vert))
                        {
                            // transformazione della lettera nel numero
                            SwitchLettere(colonna, colonnaCoord, riga, i, player);
                            //FieldShow(player); non è necessario mostrare il risultato alla fine quando si può farlo all'inizio. almeno credo anche in questo caso
                        }
                        else
                        {
                            Console.WriteLine("La casella è gia occupata");
                            continue;
                        }
                    }
                }
                else if (scelta == 2 && ship2 != 0)
                {
                    boats[i, 3] = scelta;   //MEMORIZZAZIONE LUNGHEZZA

                    if (ship2 == 0)
                    {
                        Console.WriteLine("Non sono rimaste navi di questo tipo.");
                        i--;
                    }
                    else
                    {
                        Console.WriteLine("Vuoi mettere la nave in orizzontale o verticale? (v per orizzontale, h per verticale)");
                        FrecceVisive();
                        char risp = char.Parse(Console.ReadLine());
                        if (risp == 'v')
                        {
                            vert = true;
                        }
                        else if (risp == 'h')
                        {
                            vert = false;
                        }
                        if (vert)
                        {
                            passed = false; // per rinnovare il ciclo dopo prima volta
                            while (!passed)
                            {
                                Console.WriteLine("Scegliere le coordinate della barca. Inserire 10 per la coordinata 0.");
                                Console.WriteLine("Coordinata della riga (numero):");
                                riga = int.Parse(Console.ReadLine());
                                Console.WriteLine("Coordinata della colonna (lettera)");
                                colonna = char.Parse(Console.ReadLine());

                                //boats[i, 1] = Convert.ToInt32(colonna);     //memorizzare la colonna old

                                ControlBarriers(riga, colonna, scelta, vert, ship1, ship2, ship3, ship4, ref passed);
                            }
                            if (AreaLibera(player, riga, colonna, colonnaCoord, scelta, vert))
                            {
                                for (int a = 2; a > 0; a--)
                                {
                                    // transformazione della lettera nel numero
                                    SwitchLettere(colonna, colonnaCoord, riga, i, player);
                                    riga++;
                                }
                            }
                            else
                            {
                                Console.WriteLine("La casella è gia occupata.");
                                continue;
                            }
                        }
                        else if (!vert)
                        {
                            passed = false; // per rinnovare il ciclo dopo prima volta
                            while (!passed)
                            {
                                Console.WriteLine("Scegliere le coordinate della barca. Inserire 10 per la coordinata 0.");
                                Console.WriteLine("Coordinata della riga (numero):");
                                riga = int.Parse(Console.ReadLine());
                                Console.WriteLine("Coordinata della colonna (lettera)");
                                colonna = char.Parse(Console.ReadLine());

                                //boats[i, 1] = Convert.ToInt32(colonna);     //memorizzare la colonna old

                                ControlBarriers(riga, colonna, scelta, vert, ship1, ship2, ship3, ship4, ref passed);
                            }
                            if (AreaLibera(player, riga, colonna, colonnaCoord, scelta, vert))
                            {
                                for (int a = 2; a > 0; a--)
                                {
                                    // transformazione della lettera nel numero
                                    SwitchLettere(colonna, colonnaCoord, riga, i, player);
                                    colonna++;
                                }
                            }
                            else
                            {
                                Console.WriteLine("La casella è gia occupata.");
                                continue;
                            }
                        }
                        //FieldShow(player); non è necessario mostrare il risultato alla fine quando si può farlo all'inizio
                    }
                }
                else if (scelta == 3 && ship3 != 0)
                {
                    boats[i, 3] = scelta;   //MEMORIZZAZIONE LUNGHEZZA

                    if (ship3 == 0)
                    {
                        Console.WriteLine("Non sono rimaste navi di questo tipo.");
                        i--;
                    }
                    else
                    {
                        Console.WriteLine("Vuoi mettere la nave in orizzontale o verticale? (v per orizzontale, h per verticale)");
                        FrecceVisive();
                        char risp = char.Parse(Console.ReadLine());
                        if (risp == 'v')
                        {
                            vert = true;
                        }
                        else if (risp == 'h')
                        {
                            vert = false;
                        }
                        if (vert)
                        {
                            passed = false; // per rinnovare il ciclo dopo prima volta
                            while (!passed)
                            {
                                Console.WriteLine("Scegliere le coordinate della barca. Inserire 10 per la coordinata 0.");
                                Console.WriteLine("Coordinata della riga (numero):");
                                riga = int.Parse(Console.ReadLine());
                                Console.WriteLine("Coordinata della colonna (lettera)");
                                colonna = char.Parse(Console.ReadLine());

                                //boats[i, 1] = Convert.ToInt32(colonna);     //memorizzare la colonna old

                                ControlBarriers(riga, colonna, scelta, vert, ship1, ship2, ship3, ship4, ref passed);
                            }
                            if (AreaLibera(player, riga, colonna, colonnaCoord, scelta, vert))
                            {
                                for (int a = 3; a > 0; a--)
                                {
                                    // transformazione della lettera nel numero
                                    SwitchLettere(colonna, colonnaCoord, riga, i, player);
                                    riga++;
                                }
                            }
                            else
                            {
                                Console.WriteLine("La casella è gia occupata.");
                                continue;
                            }
                        }
                        else if (!vert)
                        {
                            passed = false; // per rinnovare il ciclo dopo prima volta
                            while (!passed)
                            {
                                Console.WriteLine("Scegliere le coordinate della barca. Inserire 10 per la coordinata 0.");
                                Console.WriteLine("Coordinata della riga (numero):");
                                riga = int.Parse(Console.ReadLine());
                                Console.WriteLine("Coordinata della colonna (lettera)");
                                colonna = char.Parse(Console.ReadLine());

                                //boats[i, 1] = Convert.ToInt32(colonna);     //memorizzare la colonna old

                                ControlBarriers(riga, colonna, scelta, vert, ship1, ship2, ship3, ship4, ref passed);
                            }
                            if (AreaLibera(player, riga, colonna, colonnaCoord, scelta, vert))
                            {
                                for (int a = 3; a > 0; a--)
                                {
                                    // transformazione della lettera nel numero
                                    SwitchLettere(colonna, colonnaCoord, riga, i, player);
                                    colonna++;
                                }
                            }
                            else
                            {
                                Console.WriteLine("La casella è gia occupata.");
                                continue;
                            }
                        }
                        //FieldShow(player); non è necessario mostrare il risultato alla fine quando si può farlo all'inizio
                    }
                }
                else if (scelta == 4 && ship4 != 0)
                {
                    boats[i, 3] = scelta;   //MEMORIZZAZIONE LUNGHEZZA

                    if (ship4 == 0)
                    {
                        Console.WriteLine("Non sono rimaste navi di questo tipo.");
                        i--;
                    }
                    else
                    {
                        Console.WriteLine("Vuoi mettere la nave in orizzontale o verticale? (v per orizzontale, h per verticale)");
                        FrecceVisive();
                        char risp = char.Parse(Console.ReadLine());
                        if (risp == 'v')
                        {
                            vert = true;
                        }
                        else if (risp == 'h')
                        {
                            vert = false;
                        }
                        if (vert)
                        {
                            passed = false; // per rinnovare il ciclo dopo prima volta
                            while (!passed)
                            {
                                Console.WriteLine("Scegliere le coordinate della barca. Inserire 10 per la coordinata 0.");
                                Console.WriteLine("Coordinata della riga (numero):");
                                riga = int.Parse(Console.ReadLine());
                                Console.WriteLine("Coordinata della colonna (lettera)");
                                colonna = char.Parse(Console.ReadLine());

                                boats[i, 1] = Convert.ToInt32(colonna);     //memorizzare la colonna

                                ControlBarriers(riga, colonna, scelta, vert, ship1, ship2, ship3, ship4, ref passed);
                            }
                            if (AreaLibera(player, riga, colonna, colonnaCoord, scelta, vert))
                            {
                                for (int a = 4; a > 0; a--)
                                {
                                    // transformazione della lettera nel numero
                                    SwitchLettere(colonna, colonnaCoord, riga, i, player);
                                    riga++;
                                }
                            }
                            else
                            {
                                Console.WriteLine("La casella è gia occupata.");
                                continue;
                            }
                        }
                        else if (!vert)
                        {
                            passed = false; // per rinnovare il ciclo dopo prima volta
                            while (!passed)
                            {
                                Console.WriteLine("Scegliere le coordinate della barca. Inserire 10 per la coordinata 0.");
                                Console.WriteLine("Coordinata della riga (numero):");
                                riga = int.Parse(Console.ReadLine());
                                Console.WriteLine("Coordinata della colonna (lettera)");
                                colonna = char.Parse(Console.ReadLine());

                                //boats[i, 1] = Convert.ToInt32(colonna);     //memorizzare la colonna old

                                ControlBarriers(riga, colonna, scelta, vert, ship1, ship2, ship3, ship4, ref passed);
                            }

                            if (AreaLibera(player, riga, colonna, colonnaCoord, scelta, vert))
                            {
                                for (int a = 4; a > 0; a--)
                                {
                                    // transformazione della lettera nel numero
                                    SwitchLettere(colonna, colonnaCoord, riga, i, player);
                                    colonna++;
                                }
                            }
                            else
                            {
                                Console.WriteLine("La casella è gia occupata.");
                                continue;
                            }
                        }
                        //FieldShow(player); non è necessario mostrare il risultato alla fine quando si può farlo all'inizio
                    }
                }
                else // nel caso se i navi di un tipo non sono rimaste
                {
                    Console.WriteLine("I navi di questo tipo non rimaste piu");
                    i--;
                }

                // diminuire le navi

                if (scelta == 1 && ship1 != 0 || scelta == 2 && ship2 != 0 || scelta == 3 && ship3 != 0 || scelta == 4 && ship4 != 0) // verifica se la nave era disponibile
                {
                    switch (scelta)
                    {
                        case 1: ship1--; break;
                        case 2: ship2--; break;
                        case 3: ship3--; break;
                        case 4: ship4--; break;
                    }
                }




                //memorizzare la nave inserita
                boats[i, 0] = riga;             //memorizzare la riga
                boats[i, 1] = colonnaCoord;          //memorizzare la colonna in formato numerico non corrispondente all'ascii
                if (vert)                       //memorizzare 0 o 1 se è verticale o orizzontale
                    boats[i, 2] = 1;
                else
                    boats[i, 2] = 0;
            }
        }

        /// <summary>
        /// funzione che stampa una matrice come campo di gioco
        /// </summary>
        /// <param name="player"> matrice da stampare </param>
        static void FieldShow(char[,] player)
        {
            for (int i = 0; i < player.GetLength(0); i++)
            {
                Console.WriteLine(); // obbligatorio perche senza questo riesce male
                for (int j = 0; j < player.GetLength(0); j++)
                {
                    Console.Write(" " + player[i, j]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colonna"></param>
        /// <param name="colonnaCoord"></param>
        /// <param name="riga"></param>
        /// <param name="i"></param>
        /// <param name="player"></param>
        static void SwitchLettere(int colonna, int colonnaCoord, int riga, int i, char[,] player)
        {
            switch (colonna)
            {
                case 'A':
                case 'a':
                    colonnaCoord = 1;
                    player[riga, colonnaCoord] = '█';
                    break;
                case 'B':
                case 'b':
                    colonnaCoord = 2;
                    player[riga, colonnaCoord] = '█';
                    break;
                case 'C':
                case 'c':
                    colonnaCoord = 3;
                    player[riga, colonnaCoord] = '█';
                    break;
                case 'D':
                case 'd':
                    colonnaCoord = 4;
                    player[riga, colonnaCoord] = '█';
                    break;
                case 'E':
                case 'e':
                    colonnaCoord = 5;
                    player[riga, colonnaCoord] = '█';
                    break;
                case 'F':
                case 'f':
                    colonnaCoord = 6;
                    player[riga, colonnaCoord] = '█';
                    break;
                case 'G':
                case 'g':
                    colonnaCoord = 7;
                    player[riga, colonnaCoord] = '█';
                    break;
                case 'H':
                case 'h':
                    colonnaCoord = 8;
                    player[riga, colonnaCoord] = '█';
                    break;
                case 'I':
                case 'i':
                    colonnaCoord = 9;
                    player[riga, colonnaCoord] = '█';
                    break;
                case 'J':
                case 'j':
                    colonnaCoord = 10;
                    player[riga, colonnaCoord] = '█';
                    break;
                default:
                    i--;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="riga"></param>
        /// <param name="colonna"></param>
        /// <param name="scelta"></param>
        /// <param name="vert"></param>
        /// <param name="ship1"></param>
        /// <param name="ship2"></param>
        /// <param name="ship3"></param>
        /// <param name="ship4"></param>
        /// <param name="passed"></param>
        static void ControlBarriers(int riga, char colonna, int scelta, bool vert, int ship1, int ship2, int ship3, int ship4, ref bool passed)
        {
            if (scelta == 1)
            {
                if (riga > 11 || riga < 1)
                {
                    Console.WriteLine("Hai inserito le coordinati imposibili, prova di nuovo");
                    ship1++;
                }
                else
                {
                    if (!"ABCDEFGHIJabcdefghij".Contains(colonna))
                    {
                        Console.WriteLine("Hai inserito le coordinati imposibili, prova di nuovo");
                        ship1++;
                    }
                    else
                    {
                        passed = true;
                    }
                }
            }
            if (scelta == 2)
            {
                if (vert)
                {
                    if (riga >= 10 || riga < 1)
                    {
                        Console.WriteLine("Hai inserito le coordinati imposibili, prova di nuovo");
                        ship2++;
                    }
                    else
                    {
                        if (!"ABCDEFGHIJabcdefghij".Contains(colonna))
                        {
                            Console.WriteLine("Hai inserito le coordinati imposibili, prova di nuovo");
                            ship2++;
                        }
                        else
                        {
                            passed = true;
                        }
                    }
                }
                else
                {
                    if (riga > 11 || riga < 1)
                    {
                        Console.WriteLine("Hai inserito le coordinati imposibili, prova di nuovo");
                        ship2++;
                    }
                    else
                    {
                        if (!"ABCDEFGHIabcdefghi".Contains(colonna))
                        {
                            Console.WriteLine("Hai inserito le coordinati imposibili, prova di nuovo");
                            ship2++;
                        }
                        else
                        {
                            passed = true;
                        }
                    }
                }
            }
            if (scelta == 3)
            {
                if (vert)
                {
                    if (riga >= 9 || riga < 1)
                    {
                        Console.WriteLine("Hai inserito le coordinati imposibili, prova di nuovo");
                        ship3++;
                    }
                    else
                    {
                        if (!"ABCDEFGHIJabcdefghij".Contains(colonna))
                        {
                            Console.WriteLine("Hai inserito le coordinati imposibili, prova di nuovo");
                            ship3++;
                        }
                        else
                        {
                            passed = true;
                        }
                    }
                }
                else
                {
                    if (riga > 11 || riga < 1)
                    {
                        Console.WriteLine("Hai inserito le coordinati imposibili, prova di nuovo");
                        ship3++;
                    }
                    else
                    {
                        if (!"ABCDEFGHabcdefgh".Contains(colonna))
                        {
                            Console.WriteLine("Hai inserito le coordinati imposibili, prova di nuovo");
                            ship3++;
                        }
                        else
                        {
                            passed = true;
                        }
                    }
                }
            }
            if (scelta == 4)
            {
                if (vert)
                {
                    if (riga >= 8 || riga < 1)
                    {
                        Console.WriteLine("Hai inserito le coordinati imposibili, prova di nuovo");
                        ship4++;

                    }
                    else
                    {
                        if (!"ABCDEFGHIJabcdefghij".Contains(colonna))
                        {
                            Console.WriteLine("Hai inserito le coordinati imposibili, prova di nuovo");
                            ship4++;
                        }
                        else
                        {
                            passed = true;
                        }
                    }
                }
                else
                {
                    if (riga > 11 || riga < 1)
                    {
                        Console.WriteLine("Hai inserito le coordinati imposibili, prova di nuovo");
                        ship4++;
                    }
                    else
                    {
                        if (!"ABCDEFGabcdefg".Contains(colonna))
                        {
                            Console.WriteLine("Hai inserito le coordinati imposibili, prova di nuovo");
                            ship4++;
                        }
                        else
                        {
                            passed = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// funzione in cui si piazzano le barche in posizioni e versi casuali sulla matrice passata. usata per il riempimento del campo dell'ia
        /// </summary>
        /// <param name="ia"> matrice su cui vengono piazzate le barche </param>
        static void ShipPlacementIA(char[,] ia, int[,] boats)
        {
            Random rnd = new Random();
            int[] sizes = { 1, 2, 3, 4 };
            int[] quantita = { 4, 3, 2, 1 };

            for (int tipo = 0; tipo < sizes.Length; tipo++)
            {
                int size = sizes[tipo];
                int contato = quantita[tipo];

                for (int i = 0; i < contato; i++)
                {
                    bool placed = false;

                    while (!placed)
                    {
                        bool verticale = rnd.Next(2) == 0;
                        int righe, colonne;

                        if (verticale)
                        {
                            righe = rnd.Next(1, 12 - size);
                            colonne = rnd.Next(1, 11);
                        }
                        else
                        {
                            righe = rnd.Next(1, 11);
                            colonne = rnd.Next(1, 12 - size);
                        }

                        if (AreaLibera(ia, righe, colonne, size, verticale))
                        {
                            for (int j = 0; j < size; j++)
                            {
                                int r, c;

                                if (verticale)
                                {
                                    r = righe + j;
                                    c = colonne;
                                }
                                else
                                {
                                    r = righe;
                                    c = colonne + j;
                                }

                                ia[r, c] = '█';
                            }
                            placed = true;
                        }
                    }
                }
            }
        }       //da modificare per ottenere le informazioni di una barca quando vengono generate

        //non so se dovrei cancellare questa funzione, dato che se la cancello, l'altra funzione "arealibera" darà un errore relativo ai parametri
        static bool AreaLibera(char[,] ia, int row, int col, int size, bool vertical)
        {
            if (!vertical && col + size <= 10)
            {
                if (row > 1 && ia[row - 1, col + size] == '█')
                    return false;
                if (row < 10 && ia[row + 1, col + size] == '█')
                    return false;
            }
            if (vertical && row + size <= 10)
            {
                if (col > 1 && ia[row + size, col - 1] == '█')
                    return false;
                if (col < 10 && ia[row + size, col + 1] == '█')
                    return false;
            }
            for (int i = 0; i < size; i++)
            {
                int righe, colonne;
                if (vertical)
                {
                    righe = row + i;
                    colonne = col;
                }
                else
                {
                    righe = row;
                    colonne = col + i;
                }

                if (ia[righe, colonne] == '█') return false;

                for (int deltaRighe = -1; deltaRighe <= 1; deltaRighe++)
                {
                    for (int deltaColonne = -1; deltaColonne <= 1; deltaColonne++)
                    {
                        int numRighe = righe + deltaRighe;
                        int numColonne = colonne + deltaColonne;
                        if (numRighe >= 1 && numRighe <= 10 && numColonne >= 1 && numColonne <= 10 && ia[numRighe, numColonne] == '█')
                            return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="riga"></param>
        /// <param name="colonna"></param>
        /// <param name="colonnaCoord"></param>
        /// <param name="scelta"></param>
        /// <param name="vert"></param>
        /// <returns></returns>
        static bool AreaLibera(char[,] player, int riga, int colonna, int colonnaCoord, int scelta, bool vert)
        {
            switch (colonna)
            {
                case 'A':
                case 'a':
                    colonnaCoord = 1;
                    break;
                case 'B':
                case 'b':
                    colonnaCoord = 2;
                    break;
                case 'C':
                case 'c':
                    colonnaCoord = 3;
                    break;
                case 'D':
                case 'd':
                    colonnaCoord = 4;
                    break;
                case 'E':
                case 'e':
                    colonnaCoord = 5;
                    break;
                case 'F':
                case 'f':
                    colonnaCoord = 6;
                    break;
                case 'G':
                case 'g':
                    colonnaCoord = 7;
                    break;
                case 'H':
                case 'h':
                    colonnaCoord = 8;
                    break;
                case 'I':
                case 'i':
                    colonnaCoord = 9;
                    break;
                case 'J':
                case 'j':
                    colonnaCoord = 10;
                    break;
            }

            for (int i = 0; i < scelta; i++)
            {
                int righe;
                int colonne;

                if (vert)
                {
                    righe = riga + i;
                    colonne = colonnaCoord;
                }
                else
                {
                    righe = riga;
                    colonne = colonnaCoord + i;
                }

                // controlla se non esce dalla matrice
                if (righe < 1 || righe > 10 || colonne < 1 || colonne > 10)
                {
                    return false;
                }

                // controlla le celle vicine
                for (int deltaRiga = -1; deltaRiga <= 1; deltaRiga++)
                {
                    for (int deltaColonna = -1; deltaColonna <= 1; deltaColonna++)
                    {
                        int nuovaRiga = righe + deltaRiga;
                        int nuovaColonna = colonne + deltaColonna;

                        if (nuovaRiga >= 1 && nuovaRiga <= 10 && nuovaColonna >= 1 && nuovaColonna <= 10)
                        {
                            if (player[nuovaRiga, nuovaColonna] == '█')
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// funzione con cui, dati i campi, di chi è il turno e le coordinate dello sparo, controlla se si può sparare nella casella e spara nel campo dell'avversario
        /// </summary>
        /// <param name="ia"> campo dell'ia </param>
        /// <param name="iaHidden"> campo dell'ia nascosto </param>
        /// <param name="player"> campo del giocatore </param>
        /// <param name="playerHidden"> campo del giocatore nascosto </param>
        /// <param name="colpito"> variabile utile come ritornoi per alcune funzioni (suppongo) </param>
        /// <param name="riga"> riga in cui sparare </param>
        /// <param name="colonna"> colonna in cui sparare </param>
        /// <param name="turn"> variabile con cui si determina chi sta sparando. false=ia, true=giocatore </param>
        static void Sparo(char[,] ia, char[,] iaHidden, char[,] player, char[,] playerHidden, ref bool colpito, ref int riga, ref char colonna, ref bool turn)
        {
            int colonnaCoord = 0;                               //cambiato per far si che le coordinate arrivino dalla funzione del turno del giocatore/IA
            switch (colonna) //non troppo smart (copypasta)
            {
                case 'A':
                case 'a':
                    colonnaCoord = 1;
                    break;
                case 'B':
                case 'b':
                    colonnaCoord = 2;
                    break;
                case 'C':
                case 'c':
                    colonnaCoord = 3;
                    break;
                case 'D':
                case 'd':
                    colonnaCoord = 4;
                    break;
                case 'E':
                case 'e':
                    colonnaCoord = 5;
                    break;
                case 'F':
                case 'f':
                    colonnaCoord = 6;
                    break;
                case 'G':
                case 'g':
                    colonnaCoord = 7;
                    break;
                case 'H':
                case 'h':
                    colonnaCoord = 8;
                    break;
                case 'I':
                case 'i':
                    colonnaCoord = 9;
                    break;
                case 'J':
                case 'j':
                    colonnaCoord = 10;
                    break;
            }

            if (turn)   //se turn è true allora è turno del giocatore
            {
                if (ia[riga, colonnaCoord] == '█')
                {
                    ia[riga, colonnaCoord] = 'X';                 //il campo dell'IA viene aggiornato
                    iaHidden[riga, colonnaCoord] = 'X';           //il campo dell'IA nascosto viene aggiornato
                    Console.WriteLine("\n" + "Hai colpito");
                    colpito = true;
                }
                else
                {
                    ia[riga, colonnaCoord] = 'O';
                    iaHidden[riga, colonnaCoord] = 'O';           //i campi vengono aggiornati. O significa che in quel punto si ha mancato
                    colpito = false;
                }
            }
            else    //se non è true allora è turno dell'IA
            {
                if (player[riga, colonnaCoord] == '█')
                {
                    player[riga, colonnaCoord] = 'X';                 //il campo del giocatore viene aggiornato
                    playerHidden[riga, colonnaCoord] = 'X';           //il campo del giocatore nascosto viene aggiornato
                    Console.WriteLine("Sei stato colpito");
                    colpito = true;
                }
                else
                {
                    player[riga, colonnaCoord] = 'O';
                    playerHidden[riga, colonnaCoord] = 'O';           //i campi vengono aggiornati. O significa che in quel punto si ha mancato
                    colpito = false;
                }
            }
        }

        /// <summary>
        /// variabile che serve per stampare delle frecce con cui si mostra la direzione in cui si vuole piazzare la barca
        /// </summary>
        static void FrecceVisive() //disegno con le direzioni
        {
            Console.WriteLine("   __     |                ");
            Console.WriteLine("  |  |    |       | \\      ");
            Console.WriteLine("  |  |    | ______|  \\     ");
            Console.WriteLine(" -    -   | ______   /     ");
            Console.WriteLine(" \\    /  |       | /      ");
            Console.WriteLine("   --  (V)|           (H)  ");
        }

        /// <summary>
        /// funzione del turno del giocatore
        /// </summary>
        /// <param name="ia"> campo dell'ia </param>
        /// <param name="player"> campo dell'ia nascosto </param>
        /// <param name="iaHidden"> campo del giocatore </param>
        /// <param name="playerHidden"> campo del giocatore nascosto </param>
        /// <param name="colpito"> variabile utile per ritorni di alcune funzioni </param>
        /// <param name="turn"> variabile turno. qui viene settata come true </param>
        /// <param name="riga"> riga in cui colpire </param>
        /// <param name="colonna"> colonna in cui colpire </param>
        static void TurnoGiocatore(char[,] ia, char[,] player, char[,] iaHidden, char[,] playerHidden, ref bool colpito, ref bool turn, ref int riga, ref char colonna, int[,] boatsIa)
        {
            turn = true;    //il turno è del giocatore, quindi la variabile va settata a true

            bool drowned = false;   //variabile usata per segnalare un colpito ed affondato (ci si mette il ritorno della funzione)

            do
            {
                Console.WriteLine("Il tuo campo:");
                FieldShow(player);        //mostra delle tabelle del giocatore e dell'IA nascosta
                Console.WriteLine("\n\n\n---------------------------\n\n\nIl campo del computer:");
                FieldShow(iaHidden);

                //raccolta coordinate in cui colpire in caso si debba colpire di nuovo
                Console.WriteLine("\n\n\n\n\nInserire la riga in cui sparare (numero)");
                riga = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("\nInserire la colonna in cui sparare (lettera)");
                colonna = Convert.ToChar(Console.ReadLine());

                Sparo(ia, iaHidden, player, playerHidden, ref colpito, ref riga, ref colonna, ref turn);

                drowned = ColpitoAffondato(boatsIa, ia);    //se si è verificato un colpito ed affondato nuovo
                if (drowned)
                    Console.WriteLine("\n\nCOLPITO ED AFFONDATO!!!\n\n");
            }
            while (colpito); //se si ha colpito una nave, allora fai colpire ancora

            Console.WriteLine("\n" + "Hai mancato.");
        }

        /// <summary>
        /// turno dell'ia random
        /// </summary>
        /// <param name="ia"> campo dell'ia </param>
        /// <param name="player"> campo dell'ia nascosto </param>
        /// <param name="iaHidden"> campo del giocatore </param>
        /// <param name="playerHidden"> campo del giocatore nascosto </param>
        /// <param name="colpito"> variabile utile per ritorni di alcune funzioni </param>
        /// <param name="turn"> variabile turno. qui viene settata come true </param>
        /// <param name="riga"> riga in cui colpire </param>
        /// <param name="colonna"> colonna in cui colpire </param>
        static void TurnoIARandom(char[,] ia, char[,] player, char[,] iaHidden, char[,] playerHidden, ref bool colpito, ref bool turn, ref int riga, ref char colonna, int[,] boatsPlayer)
        {
            turn = false;    //il turno è del giocatore, quindi la variabile va settata a true

            Random random = new Random(DateTime.Now.Millisecond);

            //raccolta coordinate in cui colpire
            riga = random.Next(1, 11);
            colonna = Convert.ToChar(random.Next(97, 107));

            bool drowned = false;

            do
            {
                //raccolta coordinate in cui colpire in caso si debba colpire di nuovo
                riga = random.Next(1, 11);
                colonna = Convert.ToChar(random.Next(97, 107));

                Sparo(ia, iaHidden, player, playerHidden, ref colpito, ref riga, ref colonna, ref turn); //sparo

                drowned = ColpitoAffondato(boatsPlayer, player);    //se si è verificato un colpito ed affondato nuovo
                if (drowned)
                    Console.WriteLine("\n\nCOLPITO ED AFFONDATO!!!\n\n");

                //il giocatore vede il proprio campo dopo il colpo
                Console.Clear();
                FieldShow(player);
                Console.WriteLine("\n\nPremere qualsiasi tasto per continuare");
                Console.ReadKey();
            }
            while (colpito); //se si ha colpito una nave, allora fai colpire ancora
        }

        /// <summary>
        /// schermata di titolo
        /// </summary>
        static void SchermataIniziale()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");

            Console.WriteLine("                                     ░▒▓███████▓▒░ ░▒▓██████▓▒░▒▓████████▓▒░▒▓████████▓▒░▒▓█▓▒░      ░▒▓████████▓▒░░▒▓███████▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░▒▓███████▓▒░ ░▒▓███████▓▒░ ");
            Console.WriteLine("                                     ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░      ░▒▓█▓▒░   ░▒▓█▓▒░      ░▒▓█▓▒░      ░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░        ");
            Console.WriteLine("                                     ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░      ░▒▓█▓▒░   ░▒▓█▓▒░      ░▒▓█▓▒░      ░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░        ");
            Console.WriteLine("                                     ░▒▓███████▓▒░░▒▓████████▓▒░ ░▒▓█▓▒░      ░▒▓█▓▒░   ░▒▓█▓▒░      ░▒▓██████▓▒░  ░▒▓██████▓▒░░▒▓████████▓▒░▒▓█▓▒░▒▓███████▓▒░ ░▒▓██████▓▒░  ");
            Console.WriteLine("                                     ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░      ░▒▓█▓▒░   ░▒▓█▓▒░      ░▒▓█▓▒░             ░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░▒▓█▓▒░             ░▒▓█▓▒░ ");
            Console.WriteLine("                                     ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░      ░▒▓█▓▒░   ░▒▓█▓▒░      ░▒▓█▓▒░             ░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░▒▓█▓▒░             ░▒▓█▓▒░ ");
            Console.WriteLine("                                     ░▒▓███████▓▒░░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░      ░▒▓█▓▒░   ░▒▓████████▓▒░▒▓████████▓▒░▒▓███████▓▒░░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░▒▓█▓▒░      ░▒▓███████▓▒░  ");

            // Set the console text color to yellow
            Console.ForegroundColor = ConsoleColor.Yellow;

            // Print the message with additional newlines for a bigger visual effect
            Console.WriteLine("\n\n\n                                                                                 *************************************************  ");
            Console.WriteLine("                                                                                     PREMI UN TASTO QUALSIASI PER CONTINUARE          ");
            Console.WriteLine("                                                                                 *************************************************  ");
            Console.WriteLine("\n\n\n");

            // Wait for any key press
            Console.ResetColor(); // Reset the console color to default
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// funzione per visualizzare tutti i dati di identificazione delle barche di un giocatore (giocatore o ia)
        /// </summary>
        /// <param name="boats"> barche da visualizzare </param>
        static void DebugBarca(int[,] boats)
        {
            Console.WriteLine("  RIGA | COLONNA | VERTICALE/ORIZZONTALE |   LUNGHEZZA");
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("|  " + boats[0, 0] + "  |   " + boats[0, 1] + "      |        " + boats[0, 2] + "       |  " + boats[0, 3]);
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("|  " + boats[1, 0] + "  |   " + boats[1, 1] + "      |        " + boats[1, 2] + "       |" + boats[1, 3]);
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("|  " + boats[2, 0] + "  |   " + boats[2, 1] + "      |        " + boats[2, 2] + "       |" + boats[2, 3]);
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("|  " + boats[3, 0] + "  |   " + boats[3, 1] + "      |        " + boats[3, 2] + "       |" + boats[3, 3]);
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("|  " + boats[4, 0] + "  |   " + boats[4, 1] + "      |        " + boats[4, 2] + "       |" + boats[4, 3]);
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("|  " + boats[5, 0] + "  |   " + boats[5, 1] + "      |        " + boats[5, 2] + "       |" + boats[5, 3]);
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("|  " + boats[6, 0] + "  |   " + boats[6, 1] + "      |        " + boats[6, 2] + "       |" + boats[6, 3]);
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("|  " + boats[7, 0] + "  |   " + boats[7, 1] + "      |        " + boats[7, 2] + "       |" + boats[7, 3]);
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("|  " + boats[8, 0] + "  |   " + boats[8, 1] + "      |        " + boats[8, 2] + "       |" + boats[8, 3]);
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("|  " + boats[9, 0] + "  |   " + boats[9, 1] + "      |        " + boats[9, 2] + "       |" + boats[9, 3]);

            Console.ReadKey();
        }

        /// <summary>
        /// FUNZIONE PER CONTROLLARE CHE SIA AVVENUTO UN COLPITO E AFFONDATO
        /// </summary>
        /// <param name="boats"> informazioni delle barche del giocatore a cui appartiene il campo da analizzare </param>
        /// <param name="player"> il campo da analizzare </param>
        /// <returns> true=colpitoeaffondato, false=niente </returns>
        static bool ColpitoAffondato(int[,] boats, char[,] player)
        {
            //ciclo per controllare le barche una ad una - le barche sono obbligatoriamente 10, quindi 10 cicli
            for (int j = 0; j < 10; j++)
            {
                //-----------------------------------------------------------------------------------------------------------------------
                //se la barca da analizzare è lunga 1 ( il quarto valore di boats, ovvero boats[i, 3], è obbligatoriamente la lunghezza )
                //-----------------------------------------------------------------------------------------------------------------------
                if (boats[j, 3] == 1)
                {
                    //se nelle coordinate di quella barca c'è una X (quindi la barca è distrutta), e se quel pezzo di matrice non è stato resettato
                    if ((player[(boats[j, 0]), boats[j, 1]] == 'X') &&
                        (boats[j, 0] != 0) &&
                        (boats[j, 1] != 0) &&       // boats[j,0] è la riga  -  boats[j,1] è il valore della colonna,
                        (boats[j, 2] != 0) &&
                        (boats[j, 3] != 0))
                    {
                        SetZero(boats, j);  //setta la riga a 0 così questa condizione non viene vista
                        return true;        //comunica che è avvenuto in colpito e affondato
                    }
                }

                //-----------------------------------------------------------------------------------------------------------------------
                //se la barca da analizzare è lunga 2
                //-----------------------------------------------------------------------------------------------------------------------
                else if (boats[j, 3] == 2)
                {
                    if (boats[j, 2] == 1)    //se la barca è verticale - boats[j,2] memorizza 0 se la barca è orizzontale e 1 se è verticale
                    {
                        //se la casella di partenza e la casella sotto sono X e se la matrice delle informazioni non è stata resettata
                        if ((player[(boats[j, 0]), boats[j, 1]] == 'X') &&
                            (player[(boats[j, 0]), (boats[j, 1] + 1)] == 'X') &&    //il +1 è perchè bisogna contare la casella sotto, qundi si aumenta il dato della colonna
                            (boats[j, 0] != 0) &&
                            (boats[j, 1] != 0) &&
                            (boats[j, 2] != 0) &&
                            (boats[j, 3] != 0))
                        {
                            SetZero(boats, j);
                            return true;
                        }
                    }
                    else                    //se la barca è orizzontale
                    {
                        if ((player[(boats[j, 0]), boats[j, 1]] == 'X') &&
                            (player[(boats[j, 0] + 1), (boats[j, 1])] == 'X') &&    //il +1 è perchè bisogna contare la casella affianco, qundi si aumenta il dato della riga
                            (boats[j, 0] != 0) &&
                            (boats[j, 1] != 0) &&
                            (boats[j, 2] != 0) &&
                            (boats[j, 3] != 0))
                        {
                            SetZero(boats, j);
                            return true;
                        }
                    }
                }

                //-----------------------------------------------------------------------------------------------------------------------
                //se la barca da analizzare è lunga 3
                //-----------------------------------------------------------------------------------------------------------------------
                else if (boats[j, 3] == 3)
                {
                    if (boats[j, 2] == 1)    //barca verticale
                    {
                        //se la casella di partenza e la casella sotto sono X e se la matrice delle informazioni non è stata resettata
                        if ((player[(boats[j, 0]), boats[j, 1]] == 'X') &&          //casella di generazione
                            (player[(boats[j, 0]), (boats[j, 1] + 1)] == 'X') &&    //1 casella giù
                            (player[(boats[j, 0]), (boats[j, 1] + 2)] == 'X') &&    //2 caselle giù
                            (boats[j, 0] != 0) &&
                            (boats[j, 1] != 0) &&
                            (boats[j, 2] != 0) &&
                            (boats[j, 3] != 0))
                        {
                            SetZero(boats, j);
                            return true;
                        }
                    }
                    else                    //barca orizzontale
                    {
                        //se la casella di partenza e la casella sotto sono X e se la matrice delle informazioni non è stata resettata
                        if ((player[(boats[j, 0]), boats[j, 1]] == 'X') &&          //casella di generazione
                            (player[(boats[j, 0] + 1), (boats[j, 1])] == 'X') &&    //1 casella affianco
                            (player[(boats[j, 0] + 2), (boats[j, 1])] == 'X') &&    //2 caselle affianco
                            (boats[j, 0] != 0) &&
                            (boats[j, 1] != 0) &&
                            (boats[j, 2] != 0) &&
                            (boats[j, 3] != 0))
                        {
                            SetZero(boats, j);
                            return true;
                        }
                    }

                }

                //-----------------------------------------------------------------------------------------------------------------------
                //se la barca da analizzare è lunga 4
                //-----------------------------------------------------------------------------------------------------------------------
                else if (boats[j, 3] == 4)
                {
                    if (boats[j, 2] == 1)    //barca verticale
                    {
                        //se la casella di partenza e la casella sotto sono X e se la matrice delle informazioni non è stata resettata
                        if ((player[(boats[j, 0]), boats[j, 1]] == 'X') &&          //casella di generazione
                            (player[(boats[j, 0]), (boats[j, 1] + 1)] == 'X') &&    //1 casella giù
                            (player[(boats[j, 0]), (boats[j, 1] + 2)] == 'X') &&    //2 caselle giù
                            (player[(boats[j, 0]), (boats[j, 1] + 3)] == 'X') &&    //3 caselle giù
                            (boats[j, 0] != 0) &&
                            (boats[j, 1] != 0) &&
                            (boats[j, 2] != 0) &&
                            (boats[j, 3] != 0))
                        {
                            SetZero(boats, j);
                            return true;
                        }
                    }
                    else                    //barca orizzontale
                    {
                        //se la casella di partenza e la casella sotto sono X e se la matrice delle informazioni non è stata resettata
                        if ((player[(boats[j, 0]), boats[j, 1]] == 'X') &&          //casella di generazione
                            (player[(boats[j, 0] + 1), (boats[j, 1])] == 'X') &&    //1 casella affianco
                            (player[(boats[j, 0] + 2), (boats[j, 1])] == 'X') &&    //2 caselle affianco
                            (player[(boats[j, 0] + 3), (boats[j, 1])] == 'X') &&    //3 caselle affianco
                            (boats[j, 0] != 0) &&
                            (boats[j, 1] != 0) &&
                            (boats[j, 2] != 0) &&
                            (boats[j, 3] != 0))
                        {
                            SetZero(boats, j);
                            return true;
                        }
                    }
                }
            }

            return false;   //se nessuno dei controlli ha attivato un return allora comunica che non è avvenuti un colpito e affondato
        }

        /// <summary>
        /// funzione per settare tutti i valori di una riga di una matrice a 0
        /// </summary>
        /// <param name="boats"> matrice in cui resettare i valori </param>
        /// <param name="j"> indice della riga da resettare </param>
        static void SetZero(int[,] boats, int j)
        {
            for (int i = 0; i < 4; i++)
            {
                boats[j, i] = 0;            // j = riga da resettare
            }                               // i = cella della riga in cui resettare
        }

        /// <summary>
        /// FUNZIONE DEL TURNO DELL'IA INTELLIGENTE
        /// </summary>
        /// <param name="ia"> campo dell'ia </param>
        /// <param name="player"> campo del giocatore </param>
        /// <param name="iaHidden"> campo dell'ia nascosto </param>
        /// <param name="playerHidden"> campo del giocarore nascosto </param>
        /// <param name="colpito"> variabile utile alla funzione sparo </param>
        /// <param name="turn"> variabile turno </param>
        /// <param name="riga"> variabile della riga in cui colpire </param>
        /// <param name="colonna"> variabile della colonna in cui colpire </param>
        /// <param name="boatsPlayer"> informazioni sulle barche del giocatore, usare dalla funzione colpito ed affondato </param>
        static void TurnoIaSmart(char[,] ia, char[,] player, char[,] iaHidden, char[,] playerHidden, ref bool colpito, ref bool turn, ref int riga, ref char colonna, int[,] boatsPlayer)
        {
            turn = false;
            Random casuale = new Random(DateTime.Now.Millisecond);

            bool nave_trovata = false;
            int riga_colpita = 0;
            int colonna_colpita = 0;
            char colonna_colpita_char = ' ';

            // "HUNT": spara random finché non trova una nave che non sia da 1
            while (!nave_trovata)
            {
                TurnoIARandom(ia, player, iaHidden, playerHidden, ref colpito, ref turn, ref riga, ref colonna, boatsPlayer);

                int colonna_num = 0;
                switch (char.ToUpper(colonna))
                {
                    case 'A': colonna_num = 1; break;
                    case 'B': colonna_num = 2; break;
                    case 'C': colonna_num = 3; break;
                    case 'D': colonna_num = 4; break;
                    case 'E': colonna_num = 5; break;
                    case 'F': colonna_num = 6; break;
                    case 'G': colonna_num = 7; break;
                    case 'H': colonna_num = 8; break;
                    case 'I': colonna_num = 9; break;
                    case 'J': colonna_num = 10; break;
                }

                if (player[riga, colonna_num] == 'X')
                {
                    if (ColpitoAffondato(boatsPlayer, player))
                    {
                        nave_trovata = false; // nave da 1 affondata, resta in hunt
                    }
                    else
                    {
                        nave_trovata = true;
                        riga_colpita = riga;
                        colonna_colpita = colonna_num;
                        colonna_colpita_char = colonna;
                        break;
                    }
                }
            }

            // "TARGET": cerca di affondare la nave colpita
            if (nave_trovata)
            {
                int[] spostamento_riga = { -1, 1, 0, 0 };
                int[] spostamento_colonna = { 0, 0, -1, 1 };
                int direzione_trovata = -1;

                // STEP 1: Trova la direzione della nave colpita
                for (int direzione = 0; direzione < 4; direzione++)
                {
                    int nuova_riga = riga_colpita + spostamento_riga[direzione];
                    int nuova_colonna = colonna_colpita + spostamento_colonna[direzione];
                    if (nuova_riga >= 1 && nuova_riga <= 10 && nuova_colonna >= 1 && nuova_colonna <= 10)
                    {
                        if (player[nuova_riga, nuova_colonna] == '█')
                        {
                            int temp_riga = nuova_riga;
                            char temp_colonna = (char)('A' + nuova_colonna - 1);

                            Sparo(ia, iaHidden, player, playerHidden, ref colpito, ref temp_riga, ref temp_colonna, ref turn);

                            if (player[nuova_riga, nuova_colonna] == 'X')
                            {
                                direzione_trovata = direzione;
                                if (ColpitoAffondato(boatsPlayer, player))
                                {
                                    return; // nave affondata, torna hunt
                                }
                                break;
                            }
                        }
                    }
                }

                // STEP 2: Continua a sparare nella direzione trovata
                if (direzione_trovata != -1)
                {
                    int riga_corrente = riga_colpita;
                    int colonna_corrente = colonna_colpita;
                    bool affondata = false;
                    bool acqua_trovata = false;

                    // Primo verso
                    while (true)
                    {
                        riga_corrente += spostamento_riga[direzione_trovata];
                        colonna_corrente += spostamento_colonna[direzione_trovata];

                        if (riga_corrente < 1 || riga_corrente > 10 || colonna_corrente < 1 || colonna_corrente > 10)
                            break;

                        if (player[riga_corrente, colonna_corrente] == '█')
                        {
                            int temp_riga = riga_corrente;
                            char temp_colonna = (char)('A' + colonna_corrente - 1);

                            Sparo(ia, iaHidden, player, playerHidden, ref colpito, ref temp_riga, ref temp_colonna, ref turn);

                            if (player[riga_corrente, colonna_corrente] == 'X')
                            {
                                if (ColpitoAffondato(boatsPlayer, player))
                                {
                                    affondata = true;
                                    break; // nave affondata
                                }
                            }
                        }
                        else if (player[riga_corrente, colonna_corrente] == 'O')
                        {
                            acqua_trovata = true;
                            break; // trovata acqua, esci dal while per cambiare verso
                        }
                        else
                        {
                            break;
                        }
                    }

                    // Secondo verso (se acqua_trovata e la nave non è affondata)
                    if (acqua_trovata && !affondata)
                    {
                        // calcola la direzione opposta
                        int direzione_opposta = -1;
                        if (direzione_trovata == 0) direzione_opposta = 1;
                        else if (direzione_trovata == 1) direzione_opposta = 0;
                        else if (direzione_trovata == 2) direzione_opposta = 3;
                        else if (direzione_trovata == 3) direzione_opposta = 2;

                        riga_corrente = riga_colpita;
                        colonna_corrente = colonna_colpita;

                        while (true)
                        {
                            riga_corrente += spostamento_riga[direzione_opposta];
                            colonna_corrente += spostamento_colonna[direzione_opposta];

                            if (riga_corrente < 1 || riga_corrente > 10 || colonna_corrente < 1 || colonna_corrente > 10)
                                break;

                            if (player[riga_corrente, colonna_corrente] == '█')
                            {
                                int temp_riga = riga_corrente;
                                char temp_colonna = (char)('A' + colonna_corrente - 1);

                                Sparo(ia, iaHidden, player, playerHidden, ref colpito, ref temp_riga, ref temp_colonna, ref turn);

                                if (player[riga_corrente, colonna_corrente] == 'X')
                                {
                                    if (ColpitoAffondato(boatsPlayer, player))
                                    {
                                        break; // nave affondata
                                    }
                                }
                            }
                            else if (player[riga_corrente, colonna_corrente] == 'O')
                            {
                                break; // trovato acqua anche qui
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                }
                else
                {
                    // Nessuna direzione trovata, riprova le altre caselle adiacenti
                    for (int direzione = 0; direzione < 4; direzione++)
                    {
                        int nuova_riga = riga_colpita + spostamento_riga[direzione];
                        int nuova_colonna = colonna_colpita + spostamento_colonna[direzione];
                        if (nuova_riga >= 1 && nuova_riga <= 10 && nuova_colonna >= 1 && nuova_colonna <= 10)
                        {
                            if (player[nuova_riga, nuova_colonna] == '█')
                            {
                                int temp_riga = nuova_riga;
                                char temp_colonna = (char)('A' + nuova_colonna - 1);

                                Sparo(ia, iaHidden, player, playerHidden, ref colpito, ref temp_riga, ref temp_colonna, ref turn);

                                if (player[nuova_riga, nuova_colonna] == 'X')
                                {
                                    if (ColpitoAffondato(boatsPlayer, player))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// controlla se la partita è finita. se è finita allora avvia ChiusuraPartita dicendogli chi ha vinto
        /// </summary>
        /// <param name="plr"> campo del giocatore </param>
        /// <param name="ia"> campo dell'ia </param>
        /// <param name="gameRepeat"> variabile che nel main determina l'avvio di una nuova partita </param>
        static bool VerificaFine(char[,] plr, char[,] ia, ref bool gameRepeat, ref bool gameStart)
        {
            char simboloVerifica = '█';
            bool playerBarche = false;
            bool iaBarche = false;

            foreach (char c in plr)
            {
                if (c == simboloVerifica)
                {
                    playerBarche = true;
                    break;
                }
            }

            foreach (char c in ia)
            {
                if (c == simboloVerifica)
                {
                    iaBarche = true;
                    break;
                }
            }

            // Determinazione
            if (!playerBarche)
            {
                gameRepeat = ChiusuraPartita(true, ref gameStart);  // IA vince
                return true;    //partita finita
            }
            else if (!iaBarche)
            {
                gameRepeat = ChiusuraPartita(false, ref gameStart); // Giocatore vince
                return true;    //partita finita
            }

            return false;   //la partita non è finita
        }

        /// <summary>
        /// funzione di chiusura della partita
        /// </summary>
        /// <param name="status"> variabile: false=giocatorevince true=giocatoreperde</param>
        /// <returns> se si deve avviare un'altra partita o no </returns>
        static bool ChiusuraPartita(bool status, ref bool gameStart)
        {
            char risp = ' ';

            if (!status)
            {
                Console.WriteLine("La partita è finita! Hai vinto! Vuoi riavviare il gioco? (Y/N)");
            }
            else
            {
                Console.WriteLine("La partita è finita! Hai perso! Vuoi riavviare il gioco? ('Y' per si)");
            }

            Console.WriteLine("NOTA: Inserire qualsiasi cosa che non è Y chiuderà il gioco.");

            risp = char.Parse(Console.ReadLine());

            if (risp == 'Y' || risp == 'y')
            {
                gameStart = true;   //digli di rifare la sequenza di inizio gioco dove verrà anche rifatta la re-inizializzazione delle variabili
                return true;        //vuole fare un'altra partita
            }
            else
            {
                return false;       //non vuole fare un'altra partita
            }
        }




    }
}

//vuoi mandare una mail?
