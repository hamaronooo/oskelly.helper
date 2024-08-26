using Newtonsoft.Json;

namespace oskelly.repository.Models.ProductData;

// root
public class ProductDataResponse
{
	[JsonProperty("message")] public string Message { get; set; }

	[JsonProperty("data")] public Data Data { get; set; }

	[JsonProperty("timestamp")] public long Timestamp { get; set; }

	[JsonProperty("executionTimeMillis")] public int ExecutionTimeMillis { get; set; }

	[JsonProperty("success")] public bool Success { get; set; }
}

public class AdditionalSize
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("name")] public string Name { get; set; }

	[JsonProperty("transliterateName")] public string TransliterateName { get; set; }

	[JsonProperty("image")] public string Image { get; set; }

	[JsonProperty("isRequired")] public bool IsRequired { get; set; }
}

public class Attribute
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("name")] public string Name { get; set; }

	[JsonProperty("isRequired")] public bool IsRequired { get; set; }

	[JsonProperty("showFilter")] public bool ShowFilter { get; set; }

	[JsonProperty("attributeValues")] public List<AttributeValue> AttributeValues { get; set; }

	[JsonProperty("kind")] public string Kind { get; set; }
}

public class Attribute2
{
	[JsonProperty("attributeValueId")] public int? AttributeValueId { get; set; }

	[JsonProperty("title")] public string Title { get; set; }

	[JsonProperty("value")] public string Value { get; set; }
}

public class AttributeValue
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("value")] public string Value { get; set; }

	[JsonProperty("transliterateValue")] public string TransliterateValue { get; set; }

	[JsonProperty("singularGenitiveValue")]
	public string SingularGenitiveValue { get; set; }

	[JsonProperty("pluralGenitiveValue")] public string PluralGenitiveValue { get; set; }

	[JsonProperty("ofValue")] public string OfValue { get; set; }

	[JsonProperty("icon")] public string Icon { get; set; }
}

public class AttributeValue2
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("value")] public string Value { get; set; }

	[JsonProperty("transliterateValue")] public string TransliterateValue { get; set; }

	[JsonProperty("ofValue")] public string OfValue { get; set; }

	[JsonProperty("singularGenitiveValue")]
	public string SingularGenitiveValue { get; set; }

	[JsonProperty("icon")] public string Icon { get; set; }
}

public class AttributeWithValue
{
	[JsonProperty("attribute")] public Attribute Attribute { get; set; }

	[JsonProperty("attributeValue")] public AttributeValue AttributeValue { get; set; }
}

public class Brand
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("name")] public string Name { get; set; }

	[JsonProperty("productsCount")] public int ProductsCount { get; set; }

	[JsonProperty("urlName")] public string UrlName { get; set; }

	[JsonProperty("transliterateName")] public string TransliterateName { get; set; }

	[JsonProperty("isHidden")] public bool IsHidden { get; set; }
}

public class Breadcrumb
{
	[JsonProperty("name")] public string Name { get; set; }

	[JsonProperty("url")] public string Url { get; set; }
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

public class Comment
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("text")] public string Text { get; set; }

	[JsonProperty("publishTime")] public string PublishTime { get; set; }

	[JsonProperty("publishZonedDateTime")] public int PublishZonedDateTime { get; set; }

	[JsonProperty("userId")] public int UserId { get; set; }

	[JsonProperty("user")] public string User { get; set; }

	[JsonProperty("avatar")] public string Avatar { get; set; }

	[JsonProperty("isAnswer")] public bool IsAnswer { get; set; }

	[JsonProperty("images")] public List<object> Images { get; set; }
}

public class CommentsDTO
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("productRequestId")] public object ProductRequestId { get; set; }

	[JsonProperty("publisher")] public Publisher Publisher { get; set; }

	[JsonProperty("text")] public string Text { get; set; }

	[JsonProperty("images")] public List<object> Images { get; set; }

	[JsonProperty("publishedAtTime")] public int PublishedAtTime { get; set; }

	[JsonProperty("productId")] public int ProductId { get; set; }

	[JsonProperty("replyTo")] public object ReplyTo { get; set; }

	[JsonProperty("parentCommentId")] public object ParentCommentId { get; set; }

	[JsonProperty("subComments")] public object SubComments { get; set; }

	[JsonProperty("needsTranslate")] public bool NeedsTranslate { get; set; }

	[JsonProperty("subCommentsCount")] public object SubCommentsCount { get; set; }

	[JsonProperty("productImage")] public string ProductImage { get; set; }

	[JsonProperty("editedAtTime")] public int EditedAtTime { get; set; }

	[JsonProperty("deletedAtTime")] public object DeletedAtTime { get; set; }
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
	[JsonProperty("productId")] public int ProductId { get; set; }

	[JsonProperty("categoryId")] public int CategoryId { get; set; }

	[JsonProperty("category")] public Category Category { get; set; }

	[JsonProperty("parentCategories")] public List<ParentCategory> ParentCategories { get; set; }

	[JsonProperty("breadcrumbs")] public List<Breadcrumb> Breadcrumbs { get; set; }

	[JsonProperty("brandId")] public int BrandId { get; set; }

	[JsonProperty("brand")] public Brand Brand { get; set; }

	[JsonProperty("productModel")] public ProductModel ProductModel { get; set; }

	[JsonProperty("productModelId")] public int ProductModelId { get; set; }

	[JsonProperty("name")] public string Name { get; set; }

	[JsonProperty("attributeValueIds")] public List<int> AttributeValueIds { get; set; }

	[JsonProperty("attributeWithValues")] public List<AttributeWithValue> AttributeWithValues { get; set; }

	[JsonProperty("attributes")] public List<Attribute> Attributes { get; set; }

	[JsonProperty("sizeType")] public string SizeType { get; set; }

	[JsonProperty("description")] public string Description { get; set; }

	[JsonProperty("isVintage")] public bool IsVintage { get; set; }

	[JsonProperty("conditionId")] public int ConditionId { get; set; }

	[JsonProperty("conditionName")] public string ConditionName { get; set; }

	[JsonProperty("price")] public int Price { get; set; }

	[JsonProperty("prettyPrice")] public int PrettyPrice { get; set; }

	[JsonProperty("startPrice")] public int StartPrice { get; set; }

	[JsonProperty("priceWithoutCommission")]
	public int PriceWithoutCommission { get; set; }

	[JsonProperty("commissionProc")] public int CommissionProc { get; set; }

	[JsonProperty("productState")] public string ProductState { get; set; }

	[JsonProperty("publishTimestamp")] public int PublishTimestamp { get; set; }

	[JsonProperty("createTimestamp")] public int CreateTimestamp { get; set; }

	[JsonProperty("changeTimestamp")] public int ChangeTimestamp { get; set; }

	[JsonProperty("productStateTimestamp")]
	public int ProductStateTimestamp { get; set; }

	[JsonProperty("sentToModeratorTimestamp")]
	public int SentToModeratorTimestamp { get; set; }

	[JsonProperty("sellerRecievesSum")] public int SellerRecievesSum { get; set; }

	[JsonProperty("priceUpdateSubscribersCount")]
	public int PriceUpdateSubscribersCount { get; set; }

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

	[JsonProperty("comments")] public List<Comment> Comments { get; set; }

	[JsonProperty("commentsDTO")] public List<CommentsDTO> CommentsDTO { get; set; }

	[JsonProperty("lastCommentsTree")] public List<LastCommentsTree> LastCommentsTree { get; set; }

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

	[JsonProperty("commentPostMode")] public string CommentPostMode { get; set; }

	[JsonProperty("isBeegz")] public bool IsBeegz { get; set; }

	[JsonProperty("isAvailable")] public bool IsAvailable { get; set; }

	[JsonProperty("hasSimilar")] public bool HasSimilar { get; set; }

	[JsonProperty("similarProductRequestLink")]
	public string SimilarProductRequestLink { get; set; }

	[JsonProperty("yandexPlus")] public YandexPlus YandexPlus { get; set; }

	[JsonProperty("shortSeoDescription")] public string ShortSeoDescription { get; set; }

	[JsonProperty("primaryImageUrl")] public string PrimaryImageUrl { get; set; }
}

public class Image
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("path")] public string Path { get; set; }

	[JsonProperty("order")] public int Order { get; set; }

	[JsonProperty("alt")] public string Alt { get; set; }
}

public class LastCommentsTree
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("productRequestId")] public object ProductRequestId { get; set; }

	[JsonProperty("publisher")] public Publisher Publisher { get; set; }

	[JsonProperty("text")] public string Text { get; set; }

	[JsonProperty("images")] public List<object> Images { get; set; }

	[JsonProperty("publishedAtTime")] public int PublishedAtTime { get; set; }

	[JsonProperty("productId")] public int ProductId { get; set; }

	[JsonProperty("replyTo")] public object ReplyTo { get; set; }

	[JsonProperty("parentCommentId")] public object ParentCommentId { get; set; }

	[JsonProperty("subComments")] public object SubComments { get; set; }

	[JsonProperty("needsTranslate")] public bool NeedsTranslate { get; set; }

	[JsonProperty("subCommentsCount")] public object SubCommentsCount { get; set; }

	[JsonProperty("productImage")] public string ProductImage { get; set; }

	[JsonProperty("editedAtTime")] public int EditedAtTime { get; set; }

	[JsonProperty("deletedAtTime")] public object DeletedAtTime { get; set; }
}

public class ParentCategory
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("displayName")] public string DisplayName { get; set; }

	[JsonProperty("url")] public string Url { get; set; }

	[JsonProperty("hasChildren")] public bool HasChildren { get; set; }

	[JsonProperty("icon")] public string Icon { get; set; }
}

public class ProductModel
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("name")] public string Name { get; set; }
}

public class Publisher
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("name")] public string Name { get; set; }

	[JsonProperty("nickname")] public string Nickname { get; set; }

	[JsonProperty("avatarPath")] public string AvatarPath { get; set; }

	[JsonProperty("isPro")] public bool IsPro { get; set; }

	[JsonProperty("acceptsReturns")] public bool AcceptsReturns { get; set; }

	[JsonProperty("syncSuccess")] public bool SyncSuccess { get; set; }

	[JsonProperty("firstChar")] public string FirstChar { get; set; }

	[JsonProperty("sex")] public string Sex { get; set; }

	[JsonProperty("registrationTime")] public int RegistrationTime { get; set; }

	[JsonProperty("birthDate")] public int BirthDate { get; set; }
}

public class Seller
{
	[JsonProperty("id")] public int Id { get; set; }

	[JsonProperty("name")] public string Name { get; set; }

	[JsonProperty("nickname")] public string Nickname { get; set; }

	[JsonProperty("avatarPath")] public string AvatarPath { get; set; }

	[JsonProperty("isPro")] public bool IsPro { get; set; }

	[JsonProperty("acceptsReturns")] public bool AcceptsReturns { get; set; }

	[JsonProperty("syncSuccess")] public bool SyncSuccess { get; set; }

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

public class YandexPlus
{
	[JsonProperty("points")] public int Points { get; set; }
}