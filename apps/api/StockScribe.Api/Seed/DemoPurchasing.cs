namespace StockScribe.Api.Seed;

public sealed record DemoSupplier(string Name, decimal MinOrderAmount);
public sealed record DemoProduct(string Sku, string Name);
public sealed record DemoLine(string Sku, string Unit, int Qty, decimal UnitPrice);
public sealed record DemoPurchaseOrder(string Number, string Supplier, string DeliverToBranchCode, DemoLine[] Lines);

/// <summary>Suppliers, products and the W02 approval queue from docs/sa/seed-data.md (fictional data).</summary>
public static class DemoPurchasing
{
    public const string SiamFarm = "บจก. สยามฟาร์มเฟรช";
    public const string Thanthip = "บจก. ธารทิพย์ เบฟเวอเรจ";
    public const string RungRueang = "บจก. รุ่งเรืองฟู้ดส์";

    public static readonly DemoSupplier[] Suppliers =
    [
        new(SiamFarm, 3000m),
        new(Thanthip, 3000m),
        new(RungRueang, 3000m),
    ];

    public static readonly DemoProduct[] Products =
    [
        new("MILK-UHT-1L", "นมสด UHT รสจืด 1 ลิตร"),
        new("BREAD-400", "ขนมปังแซนด์วิช 400 กรัม"),
        new("YOGURT-135", "โยเกิร์ตรสธรรมชาติ 135 กรัม"),
        new("WATER-600", "น้ำดื่ม 600 มล."),
        new("WATER-1500", "น้ำดื่ม 1.5 ลิตร"),
        new("GREENTEA-500", "ชาเขียวพร้อมดื่ม 500 มล."),
        new("NOODLE-TOMYUM", "บะหมี่กึ่งสำเร็จรูป รสต้มยำ"),
        new("SARDINE-CAN", "ปลากระป๋องในซอสมะเขือเทศ"),
        new("OYSTER-300", "ซอสหอยนางรม 300 มล."),
        new("VINEGAR-700", "น้ำส้มสายชู 700 มล."),
    ];

    /// <summary>All go to สาขา 2. Subtotals: 4,200 · 1,560 · 2,880 (manager) and 8,500 (owner, W02-B).</summary>
    public static readonly DemoPurchaseOrder[] PendingApproval =
    [
        new("PO-2026-0835", Thanthip, "B2",
        [
            new("WATER-600", "แพ็ค 12", 12, 60m),
            new("GREENTEA-500", "แพ็ค 12", 12, 250m),
            new("WATER-1500", "แพ็ค 6", 2, 240m),
        ]),
        new("PO-2026-0836", SiamFarm, "B2",
        [
            new("MILK-UHT-1L", "ลัง", 2, 420m),
            new("BREAD-400", "ห่อ", 24, 30m),
        ]),
        new("PO-2026-0837", RungRueang, "B2",
        [
            new("NOODLE-TOMYUM", "ลัง", 8, 180m),
            new("SARDINE-CAN", "ลัง", 2, 720m),
        ]),
        new("PO-2026-0838", SiamFarm, "B2",
        [
            new("MILK-UHT-1L", "ลัง", 15, 420m),
            new("YOGURT-135", "แพ็ค", 10, 52m),
            new("BREAD-400", "ห่อ", 56, 30m),
        ]),
    ];
}
