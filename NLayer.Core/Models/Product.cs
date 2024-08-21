namespace NLayer.Core.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        //public int  Category_Id { get; set; }
        //[ForeignKey("Category_Id")] //CategoryId yerine Category_Id şeklinde farklı bir isimlendirme ile foreign belirlemek istersen, Navigation propertinin başına ForeignKey[("Category_Id)] şeklinde bunu belirtmen gerekir.

        //Referans tipli propertylerinin altını yeşille çizmesinin nedeni , bu propertyler null değer alabilir . ? (Category? gibi) koyarak null değer alabileceğini söyleyebiliriz. Ya da ctor'da null değer alırsa hata fırlat diyebilirsin(null değer alabilen tüm propertleri için). ya da "show potentiel fixed" yardımıyla çöz. ya da katman propertisinde nullable disable seç.
        public Category Category { get; set; }
        public ProductFeature ProductFeature { get; set; }
    }
}
