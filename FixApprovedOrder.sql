--Chạy Query này để fix duyệt đơn hàng 
ALTER TABLE Orders DROP CONSTRAINT CK__Orders__Status__6754599E;
ALTER TABLE Orders ADD CONSTRAINT CK_Order_Status CHECK (Status IN ('Pending', 'Processing', 'Approved', 'Delivered', 'Cancelled'));