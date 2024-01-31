namespace Web_Reviews.Services;

public class ImageConverter
{
    //примерно так
    //public byte[] GetBytes(int id) => db.Table.Where(x => x.Id == id).Select(x => x.Photo).FirstOrDefault();
    
    /*
     пробовал для Dapper'а, для EF, возможно, подойдет просто byte[]
    public string PhotoForAdd()
    {
        //пример из WPF
        ...
        bytes[] bytes = File.ReadAllBytes(openFileDialog.FileName); //считывает байты из файла
        string photo = "0x" + BitConverter.ToString(bytes).Replace("-", ""); //переводит в 16-ричную систему
        return photo; //для добавления в БД
    }
    */
}