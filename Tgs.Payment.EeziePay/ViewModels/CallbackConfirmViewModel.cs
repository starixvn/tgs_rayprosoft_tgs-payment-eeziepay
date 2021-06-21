using System.Xml.Serialization;

namespace Tgs.Payment.EeziePay.ViewModels
{
    [XmlRoot("xml", Namespace = "", IsNullable = false)]
    public class CallbackConfirmViewModel
    {
        [XmlElement("billno")]
        public string BillNo { get; set; } = default!;

        [XmlElement("status")]
        public string Status { get; set; } = default!;
    }
}