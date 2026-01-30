IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__EFMigrationsHistory]') AND type in (N'U'))
DROP TABLE [dbo].[__EFMigrationsHistory]
GO

--
-- Identity
--
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Actors]') AND type in (N'U'))
DROP TABLE [dbo].[Actors]
GO

--
-- Content
--
-- Tagging
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tags]') AND type in (N'U'))
DROP TABLE [dbo].[Tags]
GO

--
-- Chat
--
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChatMessages]') AND type in (N'U'))
DROP TABLE [dbo].[ChatMessages]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChatSessions]') AND type in (N'U'))
DROP TABLE [dbo].[ChatSessions]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TextAudio]') AND type in (N'U'))
DROP TABLE [dbo].[TextAudio]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TextImages]') AND type in (N'U'))
DROP TABLE [dbo].[TextImages]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TextPrompts]') AND type in (N'U'))
DROP TABLE [dbo].[TextPrompts]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TextResponses]') AND type in (N'U'))
DROP TABLE [dbo].[TextResponses]
GO