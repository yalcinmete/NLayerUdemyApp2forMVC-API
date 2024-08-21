using System.ComponentModel.DataAnnotations;

namespace NLayer.Core.Models
{
    //classı abstract yapalım ki bu classtan bir nesne örneği alınmasın. Abstractlar Soyut nesnelerdir.
    public abstract class BaseEntity
    {
        [Key] //Id yerine başka bir isimle kullandığın propertinin primarykey olmasını istersen başına [Key] koyman gerekir.
        //public int _Id { get; set; }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }

        //UploadDate ilk başta İlk kayıt oluşurken null değer alabilmeli.Bu nedenle DateTime? yaptık.
        public DateTime? UploadDate { get; set; }
    }
}
