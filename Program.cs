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

            //altre variabili
            bool turn = true;           //variabile turno - cambia a seconda di chi è il turno (true è il giocatore - false è l'IA)
            bool difficoltà = false;    //variabile difficoltà
            bool gameRepeat = true;     //variabile per la ripetizione del gioco
            bool gameStart = true;      //variabile che dice se la partita è appena iniziata o no. utilizzata per la selezione della difficoltà e per il posizionamento delle barche
            bool colpito = false;



            //placeholder: schermata di titolo

            /*       ciclo partita messo come commento per il debugging

            //ciclo partita
            while (gameRepeat)
            {
                if (gameStart)   //inizio partita
                {
                    FieldGeneration(player);    //riempimento dei campi di gioco, giocatore, IA e quelli nascosti
                    FieldGeneration(ia);
                    FieldGeneration(iaHidden);
                    FieldGeneration(playerHidden);

                    SelezioneDifficoltà(ref difficoltà);    //selezione della difficoltà

                    ShipPlacement(player, riga, colonna);   //posizionamento barche del giocatore

                    ShipPlacementIA(ia);    //posizionemento barche dell'IA

                    gameStart = false;  //la sequenza di inizio partita è finita, quindi non deve essere ripetuta nel prossimo ciclo
                }



                TurnoGiocatore(ia, player, iaHidden, playerHidden, ref colpito, ref turn, ref riga, ref colonna);  //turno del giocatore

                //placeholder funzione turno IA (bisognerà anche mettere l'IF per associare la difficoltà al turno dell'IA random o intelligente)
            }

            */




            FieldGeneration(player);
            FieldGeneration(ia);
            FieldGeneration(iaHidden);
            FieldGeneration(playerHidden);


            ShipPlacementIA(player);
            ShipPlacementIA(ia);

            TurnoGiocatore(ia, player, iaHidden, playerHidden, ref colpito, ref turn, ref riga, ref colonna);







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

        static void ShipPlacement(char[,] player, int riga, char colonna)
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

                if (scelta < 1 || scelta > 4)
                {
                    i--;
                    Console.WriteLine("Il tipo di nave selezionato non esiste.");
                }
                else if (scelta == 1)
                {
                    if (ship1 == 0)
                    {
                        Console.WriteLine("Non sono rimaste navi di questo tipo.");
                        i--;
                    }
                    else
                    {
                        passed = false; // per rinnovare il ciclo dopo prima volta
                        while (!passed)
                        {
                            Console.WriteLine("Scegliere le coordinate della barca. Inserire 10 per la coordinata 0.");
                            Console.WriteLine("Coordinata della riga (numero):");
                            riga = int.Parse(Console.ReadLine());
                            Console.WriteLine("Coordinata della colonna (lettera)");
                            colonna = char.Parse(Console.ReadLine());

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
                else if (scelta == 2)
                {
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
                else if (scelta == 3)
                {
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
                else if (scelta == 4)
                {
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
                // diminuire le navi

                switch (scelta)
                {
                    case 1: ship1--; break;
                    case 2: ship2--; break;
                    case 3: ship3--; break;
                    case 4: ship4--; break;
                }
            }
        }

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

        static void ShipPlacementIA(char[,] ia)
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
        }

		static bool AreaLibera(char[,] ia, int row, int col, int size, bool vertical)
        {
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

                for (int dr = -1; dr <= 1; dr++)
                {
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        int numRighe = righe + dr;
                        int numColonne = colonne + dc;
                        if (numRighe >= 1 && numRighe <= 10 && numColonne >= 1 && numColonne <= 10 && ia[numRighe, numColonne] == '█')
                            return false;
                    }
                }
            }
            return true;
        }

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
                if (vert)
                {
                    if (player[riga + i, colonnaCoord] == '█')
                        return false;
                    if (riga != 10)
                    {
                        if (player[(riga + 1) + i, colonnaCoord] == '█')
                            return false;
                        if (player[(riga + 1) + i, (colonnaCoord - 1)] == '█')
                            return false;
                        if (colonnaCoord != 10)
                        {
                            if (player[(riga + 1) + i, (colonnaCoord + 1)] == '█')
                                return false;
                        }
                    }
                    if (riga != 1)
                    {
                        if (player[(riga - 1) + i, colonnaCoord] == '█')
                            return false;
                        if (player[(riga - 1) + i, (colonnaCoord - 1)] == '█')
                            return false;
                        if (colonnaCoord != 10)
                        {
                            if (player[(riga - 1) + i, (colonnaCoord + 1)] == '█')
                                return false;
                        }
                    }
                    if (player[riga + i, (colonnaCoord - 1)] == '█')
                        return false;
                    if (colonnaCoord != 10)
                    {
                        if (player[riga + i, (colonnaCoord + 1)] == '█')
                            return false;
                    }
                }
                else
                {
                    if (player[riga, colonnaCoord + i] == '█')
                        return false;
                    if (riga != 10)
                    {
                        if (player[(riga + 1), colonnaCoord + i] == '█')
                            return false;
                        if (player[(riga + 1), (colonnaCoord - 1) + i] == '█')
                            return false;
                        if (colonnaCoord != 10)
                        {
                            if (colonnaCoord != 10)
                            {
                                if (scelta == 2)
                                {
                                    if (colonnaCoord == 9)
                                    {
                                        if (player[(riga + 1), (colonnaCoord) + i] == '█')
                                            return false;
                                    }
                                }
                                else if (scelta == 3)
                                {
                                    if (colonnaCoord == 8)
                                    {
                                        if (player[(riga + 1), (colonnaCoord) + i] == '█')
                                            return false;
                                    }
                                }
                                else if (scelta == 4)
                                {
                                    if (colonnaCoord == 7)
                                    {
                                        if (player[(riga + 1), (colonnaCoord) + i] == '█')
                                            return false;
                                    }
                                }
                                else
                                {
                                    if (player[(riga + 1), (colonnaCoord + 1) + i] == '█')
                                        return false;
                                }

                            }

                        }
                    }
                    if (riga != 1)
                    {
                        if (player[(riga - 1), colonnaCoord + i] == '█')
                            return false;
                        if (player[(riga - 1), (colonnaCoord - 1) + i] == '█')
                            return false;
                        if (colonnaCoord != 10)
                        {
                            if (colonnaCoord != 10)
                            {
                                if (scelta == 2)
                                {
                                    if (colonnaCoord == 9)
                                    {
                                        if (player[(riga), (colonnaCoord) + i] == '█')
                                            return false;
                                    }
                                }
                                else if (scelta == 3)
                                {
                                    if (colonnaCoord == 8)
                                    {
                                        if (player[(riga), (colonnaCoord) + i] == '█')
                                            return false;
                                    }
                                }
                                else if (scelta == 4)
                                {
                                    if (colonnaCoord == 7)
                                    {
                                        if (player[(riga), (colonnaCoord) + i] == '█')
                                            return false;
                                    }
                                }
                                else
                                {
                                    if (player[(riga), (colonnaCoord + 1) + i] == '█')
                                        return false;
                                }

                            }

                        }
                    }
                    if (player[riga, (colonnaCoord - 1) + i] == '█')
                        return false;
                    if (colonnaCoord != 10)
                    {
                        if (scelta == 2)
                        {
                            if (colonnaCoord == 9)
                            {
                                if (player[(riga), (colonnaCoord) + i] == '█')
                                    return false;
                            }
                        }
                        else if (scelta == 3)
                        {
                            if (colonnaCoord == 8)
                            {
                                if (player[(riga), (colonnaCoord) + i] == '█')
                                    return false;
                            }
                        }
                        else if (scelta == 4)
                        {
                            if (colonnaCoord == 7)
                            {
                                if (player[(riga), (colonnaCoord) + i] == '█')
                                    return false;
                            }
                        }
                        else
                        {
                            if (player[(riga), (colonnaCoord + 1) + i] == '█')
                                return false;
                        }

                    }

                }
            }
            return true;
        }

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

        static void FrecceVisive() //disegno con il direzione
        {
            Console.WriteLine("   __     |                ");
            Console.WriteLine("  |  |    |       | \\      ");
            Console.WriteLine("  |  |    | ______|  \\     ");
            Console.WriteLine(" -    -   | ______   /     ");
            Console.WriteLine(" \\    /  |       | /      ");
            Console.WriteLine("   --  (V)|           (H)  ");
        }

        static void TurnoGiocatore(char[,] ia, char[,] player, char[,] iaHidden, char[,] playerHidden, ref bool colpito, ref bool turn, ref int riga, ref char colonna)
        {
            turn = true;    //il turno è del giocatore, quindi la variabile va settata a true

            Console.WriteLine("Il tuo campo:");
            FieldShow(player);        //mostra delle tabelle del giocatore e dell'IA nascosta
            Console.WriteLine("\n\n\n---------------------------\n\n\nIl campo del computer (nascosto):");
            FieldShow(iaHidden);

            //raccolta coordinate in cui colpire
            Console.WriteLine("\n\n\n\n\n\nInserire la riga in cui sparare (numero)");
            riga = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("\nInserire la colonna in cui sparare (lettera)");
            colonna = Convert.ToChar(Console.ReadLine());

            Sparo(ia, iaHidden, player, playerHidden, ref colpito, ref riga, ref colonna, ref turn); //sparo

            while (colpito) //se si ha colpito una nave, allora fai colpire ancora
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

                //ATTENZIONE: qui va fatto il controllo di fine partita (per Jaco quando dovrà lavorare)
            }
            Console.WriteLine("\n" + "Hai mancato.");
        }

        static void TurnoIARandom(char[,] ia, char[,] player, char[,] iaHidden, char[,] playerHidden, ref bool colpito, ref bool turn, ref int riga, ref char colonna)
        {
            turn = false;    //il turno è del giocatore, quindi la variabile va settata a true

            Random random = new Random(DateTime.Now.Millisecond);

            //raccolta coordinate in cui colpire
            riga = random.Next(1, 11);
            colonna = Convert.ToChar(random.Next(97, 107));

            Sparo(ia, iaHidden, player, playerHidden, ref colpito, ref riga, ref colonna, ref turn); //sparo

            while (colpito) //se si ha colpito una nave, allora fai colpire ancora
            {
                //raccolta coordinate in cui colpire in caso si debba colpire di nuovo
                riga = random.Next(1, 11);
                colonna = Convert.ToChar(random.Next(97, 107));

                Sparo(ia, iaHidden, player, playerHidden, ref colpito, ref riga, ref colonna, ref turn); //sparo

                //ATTENZIONE: qui va fatto il controllo di fine partita (per Jaco quando dovrà lavorare)
            }
            Console.WriteLine("\n" + "Hai mancato.");
        }
    }
}





//il codice per la schermata di titolo sta nel drive di Teo
