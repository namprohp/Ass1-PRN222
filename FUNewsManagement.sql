USE [master]
GO

CREATE DATABASE [FUNewsManagement]
GO

USE [FUNewsManagement]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Category](
	[CategoryID] [smallint] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
	[CategoryDesciption] [nvarchar](250) NOT NULL,
	[ParentCategoryID] [smallint] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NewsArticle](
	[NewsArticleID] [nvarchar](20) NOT NULL,
	[NewsTitle] [nvarchar](400) NULL,
	[Headline] [nvarchar](150) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[NewsContent] [nvarchar](4000) NULL,
	[NewsSource] [nvarchar](400) NULL,
	[CategoryID] [smallint] NULL,
	[NewsStatus] [bit] NULL,
	[CreatedByID] [smallint] NULL,
	[UpdatedByID] [smallint] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_NewsArticle] PRIMARY KEY CLUSTERED 
(
	[NewsArticleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NewsTag](
	[NewsArticleID] [nvarchar](20) NOT NULL,
	[TagID] [int] NOT NULL,
 CONSTRAINT [PK_NewsTag] PRIMARY KEY CLUSTERED 
(
	[NewsArticleID] ASC,
	[TagID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SystemAccount](
	[AccountID] [smallint] NOT NULL,
	[AccountName] [nvarchar](100) NULL,
	[AccountEmail] [nvarchar](70) NULL,
	[AccountRole] [int] NULL,
	[AccountPassword] [nvarchar](70) NULL,
 CONSTRAINT [PK_SystemAccount] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Tag](
	[TagID] [int] NOT NULL,
	[TagName] [nvarchar](50) NULL,
	[Note] [nvarchar](400) NULL,
 CONSTRAINT [PK_HashTag] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[Category] ON 
GO

INSERT [dbo].[Category] ([CategoryID], [CategoryName], [CategoryDesciption], [ParentCategoryID], [IsActive]) 
VALUES (1, N'Tin tức học thuật', N'Danh mục này bao gồm các bài viết về kết quả nghiên cứu, bổ nhiệm và thăng chức giảng viên, và các thông báo liên quan đến học thuật.', 1, 1)
GO
INSERT [dbo].[Category] ([CategoryID], [CategoryName], [CategoryDesciption], [ParentCategoryID], [IsActive]) 
VALUES (2, N'Sinh viên', N'Danh mục này bao gồm các bài viết về hoạt động sinh viên, sự kiện và sáng kiến, như các câu lạc bộ, tổ chức và thể thao sinh viên.', 2, 1)
GO
INSERT [dbo].[Category] ([CategoryID], [CategoryName], [CategoryDesciption], [ParentCategoryID], [IsActive]) 
VALUES (3, N'An toàn khuôn viên', N'Danh mục này bao gồm các bài viết về các sự cố và biện pháp an toàn được triển khai trong khuôn viên để đảm bảo an toàn cho sinh viên và giảng viên.', 3, 1)
GO
INSERT [dbo].[Category] ([CategoryID], [CategoryName], [CategoryDesciption], [ParentCategoryID], [IsActive]) 
VALUES (4, N'Tin tức cựu sinh viên', N'Danh mục này bao gồm các bài viết về thành tựu và thành công của cựu sinh viên, như tốt nghiệp, thăng chức và thành công trong sự nghiệp.', 4, 1)
GO
INSERT [dbo].[Category] ([CategoryID], [CategoryName], [CategoryDesciption], [ParentCategoryID], [IsActive]) 
VALUES (5, N'Tin tức dự án tốt nghiệp', N'Danh mục này thường là báo cáo chi tiết được tạo ra như một phần của dự án tốt nghiệp học thuật hoặc chuyên nghiệp.', 5, 0)
GO

SET IDENTITY_INSERT [dbo].[Category] OFF
GO

INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) 
VALUES (N'1', N'Đại học FU kỷ niệm thành công của cựu sinh viên trong nhiều lĩnh vực', N'Đại học FU kỷ niệm thành công của cựu sinh viên trong nhiều lĩnh vực', CAST(N'2024-05-05T00:00:00.000' AS DateTime), 
N'Đại học FU gần đây đã kỷ niệm những thành tựu của các cựu sinh viên xuất sắc, những người đã đạt được thành công trong nhiều lĩnh vực, thể hiện tác động của nền giáo dục của trường đến hành trình nghề nghiệp của họ.

Câu chuyện thành công đa dạng: Từ các doanh nhân thành đạt đến các nghệ sĩ nổi tiếng, cựu sinh viên của Đại học FU đã đạt được những bước tiến quan trọng trong nhiều ngành công nghiệp, phản ánh sự đa dạng của nền giáo dục được cung cấp.

Cơ hội kết nối: Mạng lưới cựu sinh viên mạnh mẽ của trường đã đóng vai trò quan trọng trong việc thúc đẩy các mối quan hệ và hợp tác giữa các sinh viên tốt nghiệp, góp phần vào thành công của họ.

Đóng góp của cựu sinh viên: Nhiều cựu sinh viên cũng đã đóng góp cho trường thông qua các chương trình cố vấn, học bổng và các bài giảng khách mời, làm phong phú thêm trải nghiệm của sinh viên hiện tại.', 
N'Không có', 4, 1, 1, 1, CAST(N'2024-05-05T00:00:00.000' AS DateTime))
GO

INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) 
VALUES (N'2', N'Hiệp hội cựu sinh viên ra mắt chương trình cố vấn cho sinh viên mới tốt nghiệp', N'Hiệp hội cựu sinh viên ra mắt chương trình cố vấn cho sinh viên mới tốt nghiệp', CAST(N'2024-05-05T00:00:00.000' AS DateTime), 
N'Hiệp hội Cựu sinh viên của Đại học FU gần đây đã công bố một chương trình cố vấn mới nhằm cung cấp hỗ trợ và hướng dẫn cho các sinh viên mới tốt nghiệp khi họ chuyển từ môi trường học thuật sang thế giới nghề nghiệp.

Chương trình này ghép đôi các sinh viên mới tốt nghiệp với các cựu sinh viên giàu kinh nghiệm làm cố vấn dựa trên sở thích và mục tiêu nghề nghiệp của họ, đảm bảo sự hướng dẫn phù hợp cho từng người.

Ngoài việc cố vấn một-một, chương trình còn cung cấp các hội thảo về xây dựng hồ sơ, chuẩn bị phỏng vấn và chiến lược kết nối để trang bị cho sinh viên những kỹ năng cần thiết để thành công.

Chương trình cố vấn cũng tổ chức các sự kiện kết nối nơi các sinh viên có thể mở rộng mối quan hệ nghề nghiệp và học hỏi từ các cựu sinh viên đã xuất sắc trong lĩnh vực của họ.

Bằng cách xây dựng một mạng lưới hỗ trợ của các cố vấn cựu sinh viên, chương trình nhằm trao quyền cho các sinh viên mới tốt nghiệp để vượt qua các thách thức của thị trường việc làm, nâng cao sự phát triển nghề nghiệp và xây dựng các mối quan hệ lâu dài trong ngành của họ.

Việc ra mắt chương trình cố vấn này bởi Hiệp hội Cựu sinh viên của Đại học FU nhấn mạnh cam kết trong việc nuôi dưỡng thành công của sinh viên sau khi tốt nghiệp, tạo ra một cộng đồng hỗ trợ và hướng dẫn mạnh mẽ cho thế hệ chuyên gia tiếp theo.', 
N'Internet', 4, 1, 1, 1, CAST(N'2024-05-05T00:00:00.000' AS DateTime))
GO

INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) 
VALUES (N'3', N'Khoa học thuật công bố các sáng kiến đột phá và cải tiến chương trình', N'Khoa học thuật công bố các sáng kiến đột phá và cải tiến chương trình', CAST(N'2024-05-05T00:00:00.000' AS DateTime), 
N'Khoa Kỹ thuật Phần mềm tại FU đã công bố một loạt các sáng kiến đổi mới và cải tiến chương trình nhằm làm phong phú trải nghiệm học thuật và thúc đẩy đổi mới trong Phát triển Phần mềm.

Khoa đã thành lập [các trung tâm hoặc cơ sở nghiên cứu cụ thể] dành riêng cho việc nâng cao kiến thức và giải quyết các thách thức cấp bách trong Phát triển Phần mềm.

Thăng chức giảng viên: Một số giảng viên xuất sắc đã được thăng chức vào các vị trí lãnh đạo quan trọng, phản ánh cam kết của họ đối với sự xuất sắc trong học thuật và tầm nhìn của họ trong việc định hình tương lai của Phát triển Phần mềm.

Các chương trình học thuật trong khoa đã được cải tiến để tích hợp những phát triển mới nhất và trang bị cho sinh viên các kỹ năng thực tiễn và kiến thức phù hợp với nhu cầu hiện tại của ngành.

Những sáng kiến này sẽ định vị Khoa Kỹ thuật Phần mềm như một trung tâm đổi mới và nghiêm túc trong học thuật, thu hút nhân tài hàng đầu và thúc đẩy nghiên cứu và trải nghiệm học tập đột phá.', 
N'Không có', 1, 1, 2, 2, CAST(N'2024-05-05T00:00:00.000' AS DateTime))
GO

INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) 
VALUES (N'4', N'Học giả nổi tiếng được bổ nhiệm làm trưởng khoa AI tại FU', N'Học giả nổi tiếng được bổ nhiệm làm trưởng khoa AI tại FU', CAST(N'2024-05-05T00:00:00.000' AS DateTime), 
N'FU tự hào công bố việc bổ nhiệm David Nitzevet, một học giả xuất sắc trong lĩnh vực Học máy, vào vị trí danh giá Trưởng khoa AI, nhấn mạnh cam kết của trường đối với sự xuất sắc và lãnh đạo trong học thuật.

David Nitzevet mang đến một bề dày kinh nghiệm và chuyên môn cho vai trò này, với thành tích nổi bật trong việc phát triển các thuật toán học sâu và ứng dụng học máy trong y tế, tài chính và tiếp thị. Khi nhận nhiệm vụ, David Nitzevet bày tỏ tầm nhìn phát triển các mô hình thuật toán mới, cải thiện kỹ thuật xử lý dữ liệu trước và nâng cao khả năng diễn giải của các mô hình học máy, phù hợp với sứ mệnh của trường trong việc thúc đẩy đổi mới và xuất sắc trong Học máy.

Việc bổ nhiệm này được kỳ vọng sẽ thúc đẩy các hợp tác và sáng kiến làm phong phú cảnh quan học thuật và nghiên cứu của trường cũng như xa hơn.

Sự bổ sung David Nitzevet vào đội ngũ giảng viên Khoa AI nâng cao vị thế học thuật của trường và hứa hẹn sẽ truyền cảm hứng cho sinh viên, học giả và chuyên gia trong lĩnh vực Học máy. Việc bổ nhiệm này tái khẳng định cam kết của trường trong việc tuyển dụng nhân tài hàng đầu và nuôi dưỡng một môi trường nơi sự xuất sắc trong học thuật phát triển.', 
N'Không có', 1, 1, 2, 2, CAST(N'2024-05-05T00:00:00.000' AS DateTime))
GO

INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) 
VALUES (N'5', N'Phát hiện nghiên cứu mới làm sáng tỏ STEM', N'Phát hiện nghiên cứu mới làm sáng tỏ STEM', CAST(N'2024-05-05T00:00:00.000' AS DateTime), 
N'Nghiên cứu đột phá được thực hiện bởi Khoa Nghiên cứu của FU đã công bố những phát hiện quan trọng trong lĩnh vực STEM, mang lại những hiểu biết mới có thể cách mạng hóa sự hiểu biết và thực hành hiện tại.

Thành công của nghiên cứu này được quy cho nỗ lực hợp tác của một đội ngũ đa ngành, thể hiện cam kết của trường trong việc thúc đẩy nghiên cứu tiên tiến. Kiến thức mới được phát hiện có tiềm năng ảnh hưởng đến Toán học, Kỹ thuật, Công nghệ và định hình các nỗ lực nghiên cứu trong tương lai, định vị Khoa Nghiên cứu của FU là một nhà lãnh đạo trong sự tiến bộ của STEM.

Những phát hiện nghiên cứu này là minh chứng cho sự cống hiến của trường đối với nghiên cứu có tác động và đóng góp của nó vào cơ sở tri thức toàn cầu trong STEM.', 
N'Không có', 1, 1, 2, 2, CAST(N'2024-05-05T00:00:00.000' AS DateTime))
GO

INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'1', 5)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'1', 7)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'1', 9)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'2', 5)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'2', 7)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'2', 8)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'2', 9)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'3', 1)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'3', 8)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'3', 9)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'4', 1)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'4', 4)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'4', 8)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'4', 9)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'5', 2)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'5', 3)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'5', 4)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'5', 6)
GO

INSERT [dbo].[SystemAccount] ([AccountID], [AccountName], [AccountEmail], [AccountRole], [AccountPassword]) 
VALUES (1, N'Nguyễn Thị Mai', N'mai.nguyen@fpt.edu.vn', 2, N'@1')
GO
INSERT [dbo].[SystemAccount] ([AccountID], [AccountName], [AccountEmail], [AccountRole], [AccountPassword]) 
VALUES (2, N'Trần Văn Hùng', N'hung.tran@fpt.edu.vn', 2, N'@1')
GO
INSERT [dbo].[SystemAccount] ([AccountID], [AccountName], [AccountEmail], [AccountRole], [AccountPassword]) 
VALUES (3, N'Phạm Ngọc Lan', N'lan.pham@fpt.edu.vn', 1, N'@1')
GO
INSERT [dbo].[SystemAccount] ([AccountID], [AccountName], [AccountEmail], [AccountRole], [AccountPassword]) 
VALUES (4, N'Lê Minh Tuấn', N'tuan.le@fpt.edu.vn', 1, N'@1')
GO
INSERT [dbo].[SystemAccount] ([AccountID], [AccountName], [AccountEmail], [AccountRole], [AccountPassword]) 
VALUES (5, N'Hoàng Anh Dũng', N'dung.hoang@fpt.edu.vn', 1, N'@1')
GO

INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (1, N'Giáo dục', N'Ghi chú về Giáo dục')
GO
INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (2, N'Công nghệ', N'Ghi chú về Công nghệ')
GO
INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (3, N'Nghiên cứu', N'Ghi chú về Nghiên cứu')
GO
INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (4, N'Đổi mới', N'Ghi chú về Đổi mới')
GO
INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (5, N'Cuộc sống khuôn viên', N'Ghi chú về Cuộc sống khuôn viên')
GO
INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (6, N'Giảng viên', N'Thành tựu Giảng viên')
GO
INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (7, N'Cựu sinh viên', N'Tin tức Cựu sinh viên')
GO
INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (8, N'Sự kiện', N'Sự kiện Đại học')
GO
INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (9, N'Tài nguyên', N'Tài nguyên Khuôn viên')
GO

ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_Category] FOREIGN KEY([ParentCategoryID])
REFERENCES [dbo].[Category] ([CategoryID])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_Category]
GO
ALTER TABLE [dbo].[NewsArticle]  WITH CHECK ADD  CONSTRAINT [FK_NewsArticle_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([CategoryID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewsArticle] CHECK CONSTRAINT [FK_NewsArticle_Category]
GO
ALTER TABLE [dbo].[NewsArticle]  WITH CHECK ADD  CONSTRAINT [FK_NewsArticle_SystemAccount] FOREIGN KEY([CreatedByID])
REFERENCES [dbo].[SystemAccount] ([AccountID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewsArticle] CHECK CONSTRAINT [FK_NewsArticle_SystemAccount]
GO
ALTER TABLE [dbo].[NewsTag]  WITH CHECK ADD  CONSTRAINT [FK_NewsTag_NewsArticle] FOREIGN KEY([NewsArticleID])
REFERENCES [dbo].[NewsArticle] ([NewsArticleID])
GO
ALTER TABLE [dbo].[NewsTag] CHECK CONSTRAINT [FK_NewsTag_NewsArticle]
GO
ALTER TABLE [dbo].[NewsTag]  WITH CHECK ADD  CONSTRAINT [FK_NewsTag_Tag] FOREIGN KEY([TagID])
REFERENCES [dbo].[Tag] ([TagID])
GO
ALTER TABLE [dbo].[NewsTag] CHECK CONSTRAINT [FK_NewsTag_Tag]
GO
USE [master]
GO
ALTER DATABASE [FUNewsManagement] SET  READ_WRITE 
GO