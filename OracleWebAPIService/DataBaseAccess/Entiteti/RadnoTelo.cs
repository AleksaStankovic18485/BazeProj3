namespace NarodnaSkupstina.Entiteti
{
    internal abstract class RadnoTelo
    {
        internal protected virtual int Id { get; set; }
        internal protected virtual string? Tip { get; set; }
        internal protected virtual SluzbenaProstorija? SluzbenaProst { get; set; }
        internal protected virtual IList<NarodniPoslanik>? Clanovi { get; set; }
        

        //veze

        internal RadnoTelo()
        {
            Clanovi=new List<NarodniPoslanik>();
        }
    }
    internal class StalniOdbori : RadnoTelo
    {
        
    }
    internal class AnketniOdbori : RadnoTelo
    {

    }
    internal class Komisije : RadnoTelo
    {

    }
    internal class PrivremeniOdbori : RadnoTelo
    {

    }
}


