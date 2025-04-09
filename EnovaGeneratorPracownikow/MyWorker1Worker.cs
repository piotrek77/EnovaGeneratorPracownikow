using EnovaGeneratorPracownikow;
using Soneta.Business;
using Soneta.Business.UI;
using Soneta.EI;
using Soneta.Kadry;
using Soneta.Types;
using System;
using System.Linq;

[assembly: Worker(typeof(MyWorker1Worker), typeof(Pracownicy))]

namespace EnovaGeneratorPracownikow
{
    public class MyWorker1Worker
    {

        [Context]
        public MyWorker1WorkerParams @params
        {
            get;
            set;
        }


        // TODO -> Należy podmienić podany opis akcji na bardziej czytelny dla uzytkownika
        [Action("Enova generator pracownikow/Dodaj pracownikow", Mode = ActionMode.SingleSession | ActionMode.ConfirmSave | ActionMode.Progress)]
        public MessageBoxInformation ToDo()
        {


            return new MessageBoxInformation("Potwierdzasz wykonanie operacji ?")
            {
                Text = "Opis operacji",
                YesHandler = () =>
                {
                    GeneratorNazwisk generator = new GeneratorNazwisk();

                    using (var t = @params.Session.Logout(true))
                    {


                        for(int i = 0; i< @params.IloscPracownikow;i++)
                        {

                            var plecImieNazwisko = generator.LosujOsobe();



                            if (plecImieNazwisko.plec.Length>0 && plecImieNazwisko.imie.Length>0 && plecImieNazwisko.nazwisko.Length >0)
                            {
                                PracownikFirmy p = new PracownikFirmy();
                                //Pracownik p = Pracownik.Create(@params.Session, 1);
                                @params.Session.AddRow(p);
                                p.Kod = "?";

                                //PracHistoria ph = new PracHistoria(p);


                                string imie = plecImieNazwisko.imie;

                                if (imie.Length>30)
                                {
                                    imie = imie.Substring(0, 30);
                                }
                                p.Last.Imie = imie;
                                p.Last.Nazwisko = plecImieNazwisko.nazwisko;
                                p.Last.Plec = plecImieNazwisko.plec == "K" ? PłećOsoby.Kobieta : PłećOsoby.Mężczyzna;


                                //  ph.Etat.Okres = new Soneta.Types.FromTo(new Soneta.Types.Date(2023, 1, 1), Soneta.Types.Date.MaxValue);
                                p.Last.Etat.Okres = new FromTo(new Date(2023, 1, 1), Date.MaxValue);



                                Wydzial wydzial = Soneta.Kadry.KadryModule.GetInstance(@params.Session).Wydzialy.WgKodu["FIRMA"];
                                if (wydzial == null)
                                {
                                    throw new Exception("wydzial == null");
                                }

                                p.Last.Etat.Wydzial = wydzial;


                                p.Last.Etat.Stanowisko = "tester";
                                p.Last.Etat.Zaszeregowanie.Stawka = new Currency(5000m, "PLN");

                            }



                        }














                        t.Commit();
                    }
                    return "Operacja została zakończona";
                },
                NoHandler = () => "Operacja przerwana"
            };

        }









        // TODO -> Należy podmienić podany opis akcji na bardziej czytelny dla uzytkownika
        [Action("Enova generator pracownikow/Dodaj pracownikow - wariant 2", Mode = ActionMode.SingleSession | ActionMode.ConfirmSave | ActionMode.Progress)]
        public MessageBoxInformation ToDo2()
        {


            return new MessageBoxInformation("Potwierdzasz wykonanie operacji ?")
            {
                Text = "Opis operacji",
                YesHandler = () =>
                {
                    GeneratorNazwisk generator = new GeneratorNazwisk();

                    using (var t = @params.Session.Logout(true))
                    {


                        for (int i = 0; i < @params.IloscPracownikow; i++)
                        {

                            var plecImieNazwisko = generator.LosujOsobe();



                            if (plecImieNazwisko.plec.Length > 0 && plecImieNazwisko.imie.Length > 0 && plecImieNazwisko.nazwisko.Length > 0)
                            {
                                PracownikFirmy p = new PracownikFirmy();
                                //Pracownik p = Pracownik.Create(@params.Session, 1);
                                @params.Session.AddRow(p);
                                p.Kod = "?";

                                //PracHistoria ph = new PracHistoria(p);


                                string imie = plecImieNazwisko.imie;

                                if (imie.Length > 30)
                                {
                                    imie = imie.Substring(0, 30);
                                }
                                p.Last.Imie = imie;
                                p.Last.Nazwisko = plecImieNazwisko.nazwisko;
                                p.Last.Plec = plecImieNazwisko.plec == "K" ? PłećOsoby.Kobieta : PłećOsoby.Mężczyzna;


                                //fejkowa historia
                                Date data1 = new Date(2020, 1, 1);
                                PracHistoria pracHistoria1 = p.Historia.Update(data1);
                                @params.Session.AddRow(pracHistoria1);
                                ((IUpdateReason)pracHistoria1).UpdateReason = @"enovaGeneratorPracowników wariant 2 przed1";

                                Date data2 = new Date(2021, 1, 1);
                                PracHistoria pracHistoria2 = p.Historia.Update(data2);
                                @params.Session.AddRow(pracHistoria2);
                                ((IUpdateReason)pracHistoria2).UpdateReason = @"enovaGeneratorPracowników wariant 2 przed2";



                                Random r = new Random();

                                int zerojeden = r.Next(0, 2); // Generuje liczby 0 lub 1

                                if (zerojeden == 1)
                                {
                                    Date data3 = new Date(2023, 1, 1);
                                    PracHistoria pracHistoria3 = p.Historia.Update(data3);
                                    @params.Session.AddRow(pracHistoria3);
                                    ((IUpdateReason)pracHistoria3).UpdateReason = @"enovaGeneratorPracowników wariant 2 przed3";
                                }

                                Date dataod = new Date(2023, 1, 1);
                                FromTo okres = new FromTo(dataod, Date.MaxValue);


                                PracHistoria pracHistoria = p.Historia.FirstOrDefault(f => f.Aktualnosc.From == dataod);

                                if (pracHistoria == null)
                                {
                                    pracHistoria = p.Historia.Update(dataod);  //ponieważ tworzę całego pracownika, więc zakładam, że nie mam wpisu w PracHistoria z tą datą
                                    @params.Session.AddRow(pracHistoria);
                                    ((IUpdateReason)pracHistoria).UpdateReason = @"enovaGeneratorPracowników wariant 2";
                                }

                                pracHistoria.Etat.Okres = okres;
                                Wydzial wydzial = Soneta.Kadry.KadryModule.GetInstance(@params.Session).Wydzialy.WgKodu["FIRMA"];
                                pracHistoria.Etat.Wydzial = wydzial;
                                pracHistoria.Etat.Stanowisko = "tester v2";
                                /* 
                                p.Last.Etat.Okres = new FromTo(new Date(2023, 1, 1), Date.MaxValue);



                                Wydzial wydzial = Soneta.Kadry.KadryModule.GetInstance(@params.Session).Wydzialy.WgKodu["FIRMA"];
                                if (wydzial == null)
                                {
                                    throw new Exception("wydzial == null");
                                }

                                p.Last.Etat.Wydzial = wydzial;


                                p.Last.Etat.Stanowisko = "tester";
                                p.Last.Etat.Zaszeregowanie.Stawka = new Currency(5000m, "PLN");
                                */



                            }



                        }














                        t.Commit();
                    }
                    return "Operacja została zakończona";
                },
                NoHandler = () => "Operacja przerwana"
            };

        }
    }


    public class MyWorker1WorkerParams : ContextBase
    {
        public MyWorker1WorkerParams(Context context) : base(context)
        {
        }

        // TODO -> Poniższy parametr dodany dla celów poglądowych. Należy usunąć.
        public int IloscPracownikow { get; set; }
    }

}
