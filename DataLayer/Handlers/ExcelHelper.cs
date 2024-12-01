using DataLayer.Repositories.Interfaces;
using OfficeOpenXml;
namespace DataLayer.Handlers;

public class ExcelHelper
{
    private readonly IUserRepository userRepository;
    private readonly IGroupRepository groupRepository;
    private readonly ILocationRepository locationRepository;
    private readonly IPlaceRepository placeRepository;
    private readonly IProductBaseRepository productBaseRepository;
    private readonly ICategoryRepository categoryRepository;
    private readonly IProductRepository productRepository;
    private readonly IShoppingListRepository shoppingListRepository;
    private readonly IShoppingProductRepository shoppingProductRepository;
    public ExcelHelper(IGroupRepository groupRepository,
        ILocationRepository locationRepository,
        IPlaceRepository placeRepository,
        IProductBaseRepository productBaseRepository,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IShoppingListRepository shoppingListRepository,
        IShoppingProductRepository shoppingProductRepository,
        IUserRepository userRepository)
    {
        this.groupRepository = groupRepository;
        this.locationRepository = locationRepository;
        this.placeRepository = placeRepository;
        this.productBaseRepository = productBaseRepository;
        this.categoryRepository = categoryRepository;
        this.productRepository = productRepository;
        this.shoppingListRepository = shoppingListRepository;
        this.shoppingProductRepository = shoppingProductRepository;
        this.userRepository = userRepository;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public async Task<(byte[], string)> GetExcelForGroup(int groupId)
    {
        byte[] excelBytes;
        var group = await groupRepository.GetGroupAsync(groupId);
        var fileName = DateTime.Now.ToString("s") + "_" + groupId + "_Export.xlsx";
        var filePath = Path.Combine(Directory.GetCurrentDirectory() + fileName);
        using var package = new ExcelPackage(filePath);
        var usersInGroup = await userRepository.GetUserByGroupId(groupId);
        AddWorkSheet(package, usersInGroup.ToArray(),
            ["UserId", "Login", "PhoneNumber", "Email", "RegistrationDate"]);

        var locationsInGroup = await locationRepository.GetLocationsByGroupId(groupId);
        AddWorkSheet(package, locationsInGroup.ToArray(),
            ["LocationId", "Title", "Description", "Address", "GroupId"]);

        var placesInGroup = locationsInGroup
            .Select(async location => { return await placeRepository.GetPlacesByLocationIdAsync(location.LocationId); })
            .Select(t => t.Result)
            .SelectMany(t => t);
        AddWorkSheet(package, placesInGroup.ToArray(),
            ["PlaceId", "Name", "Description", "LocationId"]);

        var productsInGroup = placesInGroup
            .Select(async place => { return await productRepository.GetProductsByPlaceIdAsync(place.PlaceId); })
            .Select(t => t.Result)
            .SelectMany(t => t);
        AddWorkSheet(package, productsInGroup.ToArray(),
            ["ProductId", "ProductBaseId", "Quantity", "Price", "PurchaseDate", "ValidUntil", "PlaceId"]);
        
        excelBytes = await package.GetAsByteArrayAsync();
        return (excelBytes, fileName);
    }

    public async Task<(byte[], string)> GetBeautifulExcelReportForGroup(int groupId)
    {
        var group = await groupRepository.GetGroupForReportAsync(groupId);
        byte[] excelBytes;
        var fileName = DateTime.Now.ToString("s").ToString() + "_" + groupId + "_NICE_Export.xlsx";
        var filePath = Path.Combine(Directory.GetCurrentDirectory() + fileName);
        using var package = new ExcelPackage(filePath);

        AddWorkSheet(package, group, ["Title", "Description", "CreationDate"]);
        var worksheet = package.Workbook.Worksheets.Add("Values");

        int rowIndex = 1, colIndex = 1;
        // adding headers for locations
        worksheet.Cells[1, 1].RichText.Add("Title").Bold = true;
        worksheet.Cells[1, 2].RichText.Add("Description").Bold = true;
        worksheet.Cells[1, 3].RichText.Add("Address").Bold = true;
        var locations = group.Locations;
        for (int locationIndex = 0; locationIndex < locations.Count(); locationIndex++)
        {
            rowIndex++;
            int locationRow = rowIndex + locationIndex + 1;
            var location = locations[locationIndex];
            // adding info about location
            worksheet.Cells[rowIndex, 1].Value = location.Title;
            worksheet.Cells[rowIndex, 2].Value = location.Description;
            worksheet.Cells[rowIndex, 3].Value = location.Address;

            // adding headers for places
            worksheet.Cells[rowIndex, 4].RichText.Add("Name").Bold = true;
            worksheet.Cells[rowIndex, 5].RichText.Add("Description").Bold = true;

            // adding info about places
            var places = location.Places;
            for (int placeIndex = 0; placeIndex < places.Count(); placeIndex++)
            {
                rowIndex++;
                int placeRow = locationRow + placeIndex + 1;
                var place = places[placeIndex];

                worksheet.Cells[rowIndex, 4].Value = place.Name;
                worksheet.Cells[rowIndex, 5].Value = place.Description;

                // adding headers for products
                worksheet.Cells[rowIndex, 6].RichText.Add("Name").Bold = true;
                worksheet.Cells[rowIndex, 7].RichText.Add("Description").Bold = true;
                worksheet.Cells[rowIndex, 8].RichText.Add("Weight").Bold = true;
                worksheet.Cells[rowIndex, 9].RichText.Add("RunningOutQuantity").Bold = true;
                worksheet.Cells[rowIndex, 10].RichText.Add("Quantity").Bold = true;
                worksheet.Cells[rowIndex, 11].RichText.Add("Price").Bold = true;
                worksheet.Cells[rowIndex, 12].RichText.Add("PurchaseDate").Bold = true;
                worksheet.Cells[rowIndex, 13].RichText.Add("ValidUntil").Bold = true;

                var products = place.Products;
                int productRow = placeRow;
                for (int productIndex = 0; productIndex < products.Count(); productIndex++)
                {
                    rowIndex++;
                    productRow = productRow + productIndex + 1;
                    var product = products[productIndex];
                    worksheet.Cells[rowIndex, 6].Value = product.ProductBase.Name;
                    worksheet.Cells[rowIndex, 7].Value = product.ProductBase.Description;
                    worksheet.Cells[rowIndex, 8].Value = product.ProductBase.Weight;
                    worksheet.Cells[rowIndex, 9].Value = product.ProductBase.RunningOutQuantity;
                    worksheet.Cells[rowIndex, 10].Value = product.Quantity;
                    worksheet.Cells[rowIndex, 11].Value = product.Price;
                    worksheet.Cells[rowIndex, 12].Value = product.PurchaseDate.HasValue
                        ? product.PurchaseDate.Value.ToString("dd/M/yyyy") : "";
                    worksheet.Cells[rowIndex, 13].Value = product.ValidUntil.HasValue
                        ? product.ValidUntil.Value.ToString("dd/M/yyyy") : "";

                    // adding headers for categories
                    worksheet.Cells[rowIndex, 14].RichText.Add("Title").Bold = true;
                    worksheet.Cells[rowIndex, 15].RichText.Add("Description").Bold = true;

                    var categories = product.ProductBase.Categories;
                    int categoryRow = productRow;
                    for (int categoriesIndex = 0; categoriesIndex < categories.Count(); categoriesIndex++)
                    {
                        rowIndex++;
                        categoryRow = categoryRow + categoriesIndex + 1;
                        var category = categories[categoriesIndex];

                        worksheet.Cells[rowIndex, 14].Value = category.Title;
                        worksheet.Cells[rowIndex, 15].Value = category.Description;

                    }
                }
            }
        }
        worksheet.Columns.AutoFit();
        excelBytes = await package.GetAsByteArrayAsync();
        return (excelBytes, fileName);
    }
    
    private ExcelWorksheet AddWorkSheet<T>(ExcelPackage package, T[] data, List<string> propertiesNames)
    {
        var type = typeof(T);
        var typeName = type.Name;
        var worksheet = package.Workbook.Worksheets.Add(typeName);
        var typeProperties = type.GetProperties();
        int colIndex = 1;

        foreach (var property in typeProperties)
        {
            if (propertiesNames.Contains(property.Name))
            {
                worksheet.Cells[1, colIndex].Value = property.Name;
                colIndex++;
            }
        }
        int rowCount;
        T item;
        for (var arrayIndex = 0; arrayIndex < data.Length; arrayIndex++)
        {
            item = data[arrayIndex];
            rowCount = arrayIndex + 2;
            colIndex = 1;
            foreach (var property in typeProperties)
            {
                if (propertiesNames.Contains(property.Name))
                {
                    worksheet.Cells[rowCount, colIndex].Value = property.GetValue(item);
                    colIndex++;
                }
            }
        }
        return worksheet;
    }

    private ExcelWorksheet AddWorkSheet<T>(ExcelPackage package, T entity, List<string> propertiesNames)
    {
        var type = typeof(T);
        var typeName = type.Name;
        var worksheet = package.Workbook.Worksheets.Add(typeName);
        var typeProperties = type.GetProperties();
        int colIndex = 1;

        foreach (var property in typeProperties)
        {
            if (propertiesNames.Contains(property.Name))
            {
                worksheet.Cells[1, colIndex].Value = property.Name;
                worksheet.Cells[2, colIndex].Value = property.GetValue(entity);
                colIndex++;
            }
        }
        return worksheet;
    }
}
