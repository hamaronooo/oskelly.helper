using Newtonsoft.Json;

namespace oskelly.repository.Models.Catalog;

public class CatalogResponse
{
	[JsonProperty("message")] public string Message { get; set; }

	[JsonProperty("data")] public Data Data { get; set; }

	[JsonProperty("timestamp")] public long Timestamp { get; set; }

	[JsonProperty("executionTimeMillis")] public int ExecutionTimeMillis { get; set; }
}

public class AdditionalSize
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("name")] public string Name { get; set; }

	[JsonProperty("transliterateName")] public string TransliterateName { get; set; }

	[JsonProperty("image")] public string Image { get; set; }

	[JsonProperty("isRequired")] public bool IsRequired { get; set; }
}

public class Brand
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("name")] public string Name { get; set; }

	[JsonProperty("urlName")] public string UrlName { get; set; }

	[JsonProperty("transliterateName")] public string TransliterateName { get; set; }

	[JsonProperty("isHidden")] public bool IsHidden { get; set; }

	[JsonProperty("title")] public string Title { get; set; }

	[JsonProperty("productsCount")] public int? ProductsCount { get; set; }
}

public class Category
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("displayName")] public string DisplayName { get; set; }

	[JsonProperty("singularName")] public string SingularName { get; set; }

	[JsonProperty("singularFullName")] public string SingularFullName { get; set; }

	[JsonProperty("fullName")] public string FullName { get; set; }

	[JsonProperty("pluralName")] public string PluralName { get; set; }

	[JsonProperty("url")] public string Url { get; set; }

	[JsonProperty("hasChildren")] public bool HasChildren { get; set; }

	[JsonProperty("defaultSizeType")] public string DefaultSizeType { get; set; }

	[JsonProperty("additionalSizes")] public List<AdditionalSize> AdditionalSizes { get; set; }
}

public class Currency
{
	[JsonProperty("sign")] public string Sign { get; set; }

	[JsonProperty("isoCode")] public string IsoCode { get; set; }

	[JsonProperty("isoNumber")] public int IsoNumber { get; set; }

	[JsonProperty("selectedByDefault")] public bool SelectedByDefault { get; set; }

	[JsonProperty("active")] public bool Active { get; set; }

	[JsonProperty("base")] public bool Base { get; set; }
}

public class Data
{
	[JsonProperty("items")] public List<Item> Items { get; set; }

	[JsonProperty("totalPages")] public int TotalPages { get; set; }

	[JsonProperty("totalAmount")] public int TotalAmount { get; set; }

	[JsonProperty("itemsCount")] public int ItemsCount { get; set; }
}

public class Image
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("path")] public string Path { get; set; }

	[JsonProperty("order")] public int Order { get; set; }
}

public class Item
{
	[JsonProperty("productId")] public int ProductId { get; set; }

	[JsonProperty("categoryId")] public int CategoryId { get; set; }

	[JsonProperty("category")] public Category Category { get; set; }

	[JsonProperty("brandId")] public int BrandId { get; set; }

	[JsonProperty("brand")] public Brand Brand { get; set; }

	[JsonProperty("name")] public string Name { get; set; }

	[JsonProperty("attributeValueIds")] public List<int> AttributeValueIds { get; set; }

	[JsonProperty("sizeType")] public string SizeType { get; set; }

	[JsonProperty("sizes")] public List<Size> Sizes { get; set; }

	[JsonProperty("images")] public List<Image> Images { get; set; }

	[JsonProperty("description")] public string Description { get; set; }

	[JsonProperty("isVintage")] public bool IsVintage { get; set; }

	[JsonProperty("conditionId")] public int ConditionId { get; set; }

	[JsonProperty("conditionName")] public string ConditionName { get; set; }

	[JsonProperty("price")] public int Price { get; set; }

	[JsonProperty("prettyPrice")] public int PrettyPrice { get; set; }

	[JsonProperty("startPrice")] public int StartPrice { get; set; }

	[JsonProperty("productState")] public string ProductState { get; set; }

	[JsonProperty("publishTimestamp")] public int PublishTimestamp { get; set; }

	[JsonProperty("createTimestamp")] public int CreateTimestamp { get; set; }

	[JsonProperty("changeTimestamp")] public int ChangeTimestamp { get; set; }

	[JsonProperty("productStateTimestamp")]
	public int ProductStateTimestamp { get; set; }

	[JsonProperty("sentToModeratorTimestamp")]
	public int SentToModeratorTimestamp { get; set; }

	[JsonProperty("commentsCount")] public int CommentsCount { get; set; }

	[JsonProperty("likesCount")] public int LikesCount { get; set; }

	[JsonProperty("isOurChoice")] public bool IsOurChoice { get; set; }

	[JsonProperty("isLiked")] public bool IsLiked { get; set; }

	[JsonProperty("seller")] public Seller Seller { get; set; }

	[JsonProperty("url")] public string Url { get; set; }

	[JsonProperty("isAtOffice")] public bool IsAtOffice { get; set; }

	[JsonProperty("isNewCollection")] public bool IsNewCollection { get; set; }

	[JsonProperty("subscribedOnPriceUpdates")]
	public bool SubscribedOnPriceUpdates { get; set; }

	[JsonProperty("isUsedInSaleRequest")] public bool IsUsedInSaleRequest { get; set; }

	[JsonProperty("isReadyForBargain")] public bool IsReadyForBargain { get; set; }

	[JsonProperty("availabilityForBargainDate")]
	public DateTime AvailabilityForBargainDate { get; set; }

	[JsonProperty("sex")] public string Sex { get; set; }

	[JsonProperty("needsTranslateDescription")]
	public bool NeedsTranslateDescription { get; set; }

	[JsonProperty("currency")] public Currency Currency { get; set; }

	[JsonProperty("salesChannel")] public string SalesChannel { get; set; }

	[JsonProperty("inBoutique")] public bool InBoutique { get; set; }

	[JsonProperty("isConcierge")] public bool IsConcierge { get; set; }

	[JsonProperty("isAvailable")] public bool IsAvailable { get; set; }

	[JsonProperty("hasSimilar")] public bool HasSimilar { get; set; }

	[JsonProperty("split")] public Split Split { get; set; }

	[JsonProperty("yandexPlus")] public YandexPlus YandexPlus { get; set; }

	[JsonProperty("primaryImageUrl")] public string PrimaryImageUrl { get; set; }

	[JsonProperty("productModel")] public ProductModel ProductModel { get; set; }

	[JsonProperty("productModelId")] public int? ProductModelId { get; set; }

	[JsonProperty("purchasePrice")] public int? PurchasePrice { get; set; }

	[JsonProperty("higherPrice")] public int? HigherPrice { get; set; }

	[JsonProperty("discount")] public int? Discount { get; set; }

	[JsonProperty("prettyDiscount")] public int? PrettyDiscount { get; set; }

	[JsonProperty("rrpPrice")] public int? RrpPrice { get; set; }

	[JsonProperty("vendorCode")] public string VendorCode { get; set; }
}

public class Part
{
	[JsonProperty("date")] public DateTime Date { get; set; }

	[JsonProperty("value")] public int Value { get; set; }
}

public class ProductModel
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("name")] public string Name { get; set; }
}



public class Seller
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("name")] public string Name { get; set; }

	[JsonProperty("nickname")] public string Nickname { get; set; }

	[JsonProperty("avatarPath")] public string AvatarPath { get; set; }

	[JsonProperty("isPro")] public bool IsPro { get; set; }

	[JsonProperty("isTrusted")] public bool IsTrusted { get; set; }

	[JsonProperty("acceptsReturns")] public bool AcceptsReturns { get; set; }

	[JsonProperty("firstChar")] public string FirstChar { get; set; }

	[JsonProperty("sex")] public string Sex { get; set; }

	[JsonProperty("isFollowed")] public bool IsFollowed { get; set; }

	[JsonProperty("productsCount")] public int ProductsCount { get; set; }

	[JsonProperty("registrationTime")] public int RegistrationTime { get; set; }

	[JsonProperty("birthDate")] public int BirthDate { get; set; }
}

public class Size
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("productSizeType")] public string ProductSizeType { get; set; }

	[JsonProperty("productSizeValue")] public string ProductSizeValue { get; set; }

	[JsonProperty("categorySizeType")] public string CategorySizeType { get; set; }

	[JsonProperty("categorySizeValue")] public string CategorySizeValue { get; set; }

	[JsonProperty("count")] public int Count { get; set; }

	[JsonProperty("ordering")] public int Ordering { get; set; }
}

public class Split
{
	[JsonProperty("firstPayment")] public int FirstPayment { get; set; }

	[JsonProperty("remainingPayment")] public int RemainingPayment { get; set; }

	[JsonProperty("parts")] public List<Part> Parts { get; set; }
}

public class YandexPlus
{
	[JsonProperty("points")] public int Points { get; set; }
}