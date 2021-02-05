using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA_LANG
{
    public static class ERRORS
    {
        public const string INVALID_SYNTAX = "Geçersiz sözdizimi hatası!. Satır: "; 
        public const string INVALID_VARIABLE = "Değişken adı geçerli bağlamda bulunamadı !. Satır: ";
        public const string INVALID_OPERATION = "İşlem yapılırken hata oluştu! Lütfen sözdiziminizi kontrol edin. Satır: ";
        public const string END_OF_LINE_EXPECTING = "Hata! Satır sonu belirteci bekleniyor(;)!. Satır: ";
        public const string TYPE_CONVERSION_EXCEPTION = "Hata! Geçersiz tip dönüşümü. Satır: ";


    }
}
