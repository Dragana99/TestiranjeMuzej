namespace Museum.Domain.Common
{
    public static class Messages
    {

        #region Auditoriums
        public const string AUDITORIUM_GET_ALL_AUDITORIUMS_ERROR = "Doslo je do greske, molimo vas da pokusate kasnije.";
        public const string AUDITORIUM_PROPERTIE_NAME_NOT_VALID = "Ime izlozbene prostorije ne sme biti duze od 30 karaktera.";
        public const string AUDITORIUM_CREATION_ERROR = "Doslo je do greske prilikom kreiranja nove izlozbene sale. Molimo vas da pokusate kasnije.";
        public const string AUDITORIUM_SAME_NAME = "Nije moguce kreirati izlozbenu salu. Sala sa istim imenom vec postoji.";
        public const string AUDITORIUM_UNVALID_MUSEUMID = "Nije moguce kreirati izlozbenu salu zbog nepostojeceg ID muzeja. ";
        public const string AUDITORIUM_DOES_NOT_EXIST = "Izlozbena sala ne postoji.";
        #endregion

        #region Museum
        public const string MUSEUM_GET_ALL_MUSEUM_ERROR = "Doslo je do greske. Molimo vas pokusajte kasnije.";
        public const string MUSEUM_PROPERTY_NAME_NOT_VALID = "Naziv muzeja ne sme da bude duze od 20 karaktera.";
        public const string MUSEUM_CREATION_ERROR = "Doslo je do greske prilikom kreiranja novog muzeja. Molimo vas da pokusate kasnije.";
        public const string MUSEUM_DOES_NOT_EXIST = "Muzej ne postoji.";
        public const string MUSEUM_SAME_NAME = "Muzej pod datim imenom vec postoji";
        #endregion

        #region EXHABIT        
        public const string EXHABIT_DOES_NOT_EXIST = "Exponat ne postoji.";
        public const string EXHABIT_PROPERTIE_TITLE_NOT_VALID = "Naziv exponata ne sme da bude duze od 30 karaktera.";
        public const string EXHABIT_PROPERTIE_YEAR_NOT_VALID = "Godina otkrica eksponata je u razmaku od 0-2000. godine";
        public const string EXHABIT_CREATION_ERROR = "Doslo je do greske prilikom kreiranja novog exponata. Molimo vas da pokusate kasnije.";
        public const string EXHABIT_GET_BY_ID = "Doslo je do greske. Molimo vas pokusajte kasnije.";
        public const string EXHABIT_GET_ALL_EXHABIT_ERROR = "Doslo je do greske. Molimo vas pokusajte kasnije.";
        #endregion

        #region Exhabitions
        public const string EXHIBITIONS_GET_ALL_EXHIBITIONS_ERROR = "Doslo je do greske. Molimo vas pokusajte kasnije.";
        public const string EXHIBITIONS_CREATION_ERROR = "Doslo je do greske prilikom kreiranja nove izlozbe. Molimo vas da pokusate kasnije.";
        public const string EXHIBITIONS_AT_SAME_TIME = "Nije moguce kreirati izlozbu jer izlozba pod istim imenom vec postoji.";
        public const string EXHIBITIONS_IN_PAST = "Vreme otvaranja izlozbe ne moze da bude u proslosti";
        public const string EXHIBITION_DOES_NOT_EXIST = "Izlozba ne postoji.";
        public const string EXHIBITION_CREATION_ERROR = "Error occured while creating new exhibition, please try again.";
        public const string EXHIBITIONS_CAN_NOT_BE_DELETED_BECAUSE_TICKET_EXIST = "Nije moguce obrisati izlozbu jer ulaznice za izlozbu su vec kupljene.";
        #endregion

       

    }
}
