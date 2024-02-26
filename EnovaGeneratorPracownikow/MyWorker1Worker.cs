using EnovaGeneratorPracownikow;
using Soneta.Business;
using Soneta.Business.UI;
using Soneta.Kadry;
using Soneta.Types;
using System;

[assembly: Worker(typeof(MyWorker1Worker), typeof(Pracownik))]

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
