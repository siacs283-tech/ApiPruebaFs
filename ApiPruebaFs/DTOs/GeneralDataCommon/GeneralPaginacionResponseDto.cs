namespace ApiPruebaFs.DTOs.GeneralDataCommon
{
    public class GeneralPaginacionResponseDto<T>
    {
        public int Pagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
