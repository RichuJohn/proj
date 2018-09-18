
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/18/2018 15:53:41
-- Generated from EDMX file: C:\Users\Richu\Source\Repos\FitnessGuru-Main\FitnessGuru-Main\Models\FitnessGuruModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [FitnessGuru];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_GymMemberGroupActivity_GroupActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GymMemberGroupActivity] DROP CONSTRAINT [FK_GymMemberGroupActivity_GroupActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_GymMemberGroupActivity_GymMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GymMemberGroupActivity] DROP CONSTRAINT [FK_GymMemberGroupActivity_GymMember];
GO
IF OBJECT_ID(N'[dbo].[FK_GymMemberGroupJoinRequest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupJoinRequests] DROP CONSTRAINT [FK_GymMemberGroupJoinRequest];
GO
IF OBJECT_ID(N'[dbo].[FK_GymMemberPartnerGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PartnerGroups] DROP CONSTRAINT [FK_GymMemberPartnerGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_GymMemberPersonalAppointmentRequest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonalAppointmentRequests1] DROP CONSTRAINT [FK_GymMemberPersonalAppointmentRequest];
GO
IF OBJECT_ID(N'[dbo].[FK_GymMemberSession]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sessions] DROP CONSTRAINT [FK_GymMemberSession];
GO
IF OBJECT_ID(N'[dbo].[FK_GymMemberSession1_GymMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GymMemberSession1] DROP CONSTRAINT [FK_GymMemberSession1_GymMember];
GO
IF OBJECT_ID(N'[dbo].[FK_GymMemberSession1_Session]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GymMemberSession1] DROP CONSTRAINT [FK_GymMemberSession1_Session];
GO
IF OBJECT_ID(N'[dbo].[FK_GymMemberSessionFeedback]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SessionFeedbacks] DROP CONSTRAINT [FK_GymMemberSessionFeedback];
GO
IF OBJECT_ID(N'[dbo].[FK_GymMembersInPartnerGroup_GymMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GymMembersInPartnerGroup] DROP CONSTRAINT [FK_GymMembersInPartnerGroup_GymMember];
GO
IF OBJECT_ID(N'[dbo].[FK_GymMembersInPartnerGroup_PartnerGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GymMembersInPartnerGroup] DROP CONSTRAINT [FK_GymMembersInPartnerGroup_PartnerGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_GymMemberTrainerPersonalAppointmentSession]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TrainerPersonalAppointmentSessions] DROP CONSTRAINT [FK_GymMemberTrainerPersonalAppointmentSession];
GO
IF OBJECT_ID(N'[dbo].[FK_PartnerGroupGroupActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupActivities] DROP CONSTRAINT [FK_PartnerGroupGroupActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_PartnerGroupGroupJoinRequest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupJoinRequests] DROP CONSTRAINT [FK_PartnerGroupGroupJoinRequest];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonalAppointmentRequestTrainerPersonalAppointmentSession]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonalAppointmentRequests1] DROP CONSTRAINT [FK_PersonalAppointmentRequestTrainerPersonalAppointmentSession];
GO
IF OBJECT_ID(N'[dbo].[FK_SessionSessionFeedback]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SessionFeedbacks] DROP CONSTRAINT [FK_SessionSessionFeedback];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[GroupActivities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupActivities];
GO
IF OBJECT_ID(N'[dbo].[GroupJoinRequests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupJoinRequests];
GO
IF OBJECT_ID(N'[dbo].[GymMemberGroupActivity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GymMemberGroupActivity];
GO
IF OBJECT_ID(N'[dbo].[GymMembers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GymMembers];
GO
IF OBJECT_ID(N'[dbo].[GymMemberSession1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GymMemberSession1];
GO
IF OBJECT_ID(N'[dbo].[GymMembersInPartnerGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GymMembersInPartnerGroup];
GO
IF OBJECT_ID(N'[dbo].[PartnerGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PartnerGroups];
GO
IF OBJECT_ID(N'[dbo].[PersonalAppointmentRequests1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonalAppointmentRequests1];
GO
IF OBJECT_ID(N'[dbo].[SessionFeedbacks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SessionFeedbacks];
GO
IF OBJECT_ID(N'[dbo].[Sessions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sessions];
GO
IF OBJECT_ID(N'[dbo].[TrainerPersonalAppointmentSessions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TrainerPersonalAppointmentSessions];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'GymMembers'
CREATE TABLE [dbo].[GymMembers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [DOB] datetime  NOT NULL,
    [AddressLine1] nvarchar(max)  NULL,
    [AddressLine2] nvarchar(max)  NULL,
    [ProfilePicPath] nvarchar(max)  NULL,
    [Desc] nvarchar(max)  NULL,
    [UserId] nvarchar(max)  NOT NULL,
    [Gender] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Sessions'
CREATE TABLE [dbo].[Sessions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SessionName] nvarchar(max)  NOT NULL,
    [SessionAt] datetime  NOT NULL,
    [isCancelled] bit  NOT NULL,
    [Desc] nvarchar(max)  NOT NULL,
    [TrainerId] int  NOT NULL
);
GO

-- Creating table 'SessionFeedbacks'
CREATE TABLE [dbo].[SessionFeedbacks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(max)  NULL,
    [Rating] int  NULL,
    [GymMemberId] int  NOT NULL,
    [SessionId] int  NOT NULL
);
GO

-- Creating table 'TrainerPersonalAppointmentSessions'
CREATE TABLE [dbo].[TrainerPersonalAppointmentSessions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AvailableAt] datetime  NOT NULL,
    [TrainerId] int  NOT NULL
);
GO

-- Creating table 'PersonalAppointmentRequests1'
CREATE TABLE [dbo].[PersonalAppointmentRequests1] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(max)  NOT NULL,
    [IsApproved] bit  NOT NULL,
    [Feedback] nvarchar(max)  NULL,
    [GymMemberId] int  NOT NULL,
    [Rating] int  NULL,
    [TrainerPersonalAppointmentSession_Id] int  NOT NULL
);
GO

-- Creating table 'PartnerGroups'
CREATE TABLE [dbo].[PartnerGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Owner] int  NOT NULL
);
GO

-- Creating table 'GroupJoinRequests'
CREATE TABLE [dbo].[GroupJoinRequests] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(max)  NOT NULL,
    [PartnerGroupId] int  NOT NULL,
    [GymMemberId] int  NOT NULL
);
GO

-- Creating table 'GroupActivities'
CREATE TABLE [dbo].[GroupActivities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(max)  NOT NULL,
    [ScheduledAt] datetime  NOT NULL,
    [PartnerGroupId] int  NOT NULL
);
GO

-- Creating table 'Notifications'
CREATE TABLE [dbo].[Notifications] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(max)  NOT NULL,
    [GymMemberId] int  NOT NULL,
    [NotificationType_Id] int  NOT NULL
);
GO

-- Creating table 'NotificationTypes'
CREATE TABLE [dbo].[NotificationTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'GymMemberSession1'
CREATE TABLE [dbo].[GymMemberSession1] (
    [GymMembers_Id] int  NOT NULL,
    [JoinedSessions_Id] int  NOT NULL
);
GO

-- Creating table 'GymMembersInPartnerGroup'
CREATE TABLE [dbo].[GymMembersInPartnerGroup] (
    [GymMembers_Id] int  NOT NULL,
    [JoinedGroups_Id] int  NOT NULL
);
GO

-- Creating table 'GymMemberGroupActivity'
CREATE TABLE [dbo].[GymMemberGroupActivity] (
    [GymMembers_Id] int  NOT NULL,
    [GroupActivities_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'GymMembers'
ALTER TABLE [dbo].[GymMembers]
ADD CONSTRAINT [PK_GymMembers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Sessions'
ALTER TABLE [dbo].[Sessions]
ADD CONSTRAINT [PK_Sessions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SessionFeedbacks'
ALTER TABLE [dbo].[SessionFeedbacks]
ADD CONSTRAINT [PK_SessionFeedbacks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TrainerPersonalAppointmentSessions'
ALTER TABLE [dbo].[TrainerPersonalAppointmentSessions]
ADD CONSTRAINT [PK_TrainerPersonalAppointmentSessions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PersonalAppointmentRequests1'
ALTER TABLE [dbo].[PersonalAppointmentRequests1]
ADD CONSTRAINT [PK_PersonalAppointmentRequests1]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PartnerGroups'
ALTER TABLE [dbo].[PartnerGroups]
ADD CONSTRAINT [PK_PartnerGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GroupJoinRequests'
ALTER TABLE [dbo].[GroupJoinRequests]
ADD CONSTRAINT [PK_GroupJoinRequests]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GroupActivities'
ALTER TABLE [dbo].[GroupActivities]
ADD CONSTRAINT [PK_GroupActivities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Notifications'
ALTER TABLE [dbo].[Notifications]
ADD CONSTRAINT [PK_Notifications]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NotificationTypes'
ALTER TABLE [dbo].[NotificationTypes]
ADD CONSTRAINT [PK_NotificationTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [GymMembers_Id], [JoinedSessions_Id] in table 'GymMemberSession1'
ALTER TABLE [dbo].[GymMemberSession1]
ADD CONSTRAINT [PK_GymMemberSession1]
    PRIMARY KEY CLUSTERED ([GymMembers_Id], [JoinedSessions_Id] ASC);
GO

-- Creating primary key on [GymMembers_Id], [JoinedGroups_Id] in table 'GymMembersInPartnerGroup'
ALTER TABLE [dbo].[GymMembersInPartnerGroup]
ADD CONSTRAINT [PK_GymMembersInPartnerGroup]
    PRIMARY KEY CLUSTERED ([GymMembers_Id], [JoinedGroups_Id] ASC);
GO

-- Creating primary key on [GymMembers_Id], [GroupActivities_Id] in table 'GymMemberGroupActivity'
ALTER TABLE [dbo].[GymMemberGroupActivity]
ADD CONSTRAINT [PK_GymMemberGroupActivity]
    PRIMARY KEY CLUSTERED ([GymMembers_Id], [GroupActivities_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [TrainerId] in table 'Sessions'
ALTER TABLE [dbo].[Sessions]
ADD CONSTRAINT [FK_GymMemberSession]
    FOREIGN KEY ([TrainerId])
    REFERENCES [dbo].[GymMembers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GymMemberSession'
CREATE INDEX [IX_FK_GymMemberSession]
ON [dbo].[Sessions]
    ([TrainerId]);
GO

-- Creating foreign key on [GymMembers_Id] in table 'GymMemberSession1'
ALTER TABLE [dbo].[GymMemberSession1]
ADD CONSTRAINT [FK_GymMemberSession1_GymMember]
    FOREIGN KEY ([GymMembers_Id])
    REFERENCES [dbo].[GymMembers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [JoinedSessions_Id] in table 'GymMemberSession1'
ALTER TABLE [dbo].[GymMemberSession1]
ADD CONSTRAINT [FK_GymMemberSession1_Session]
    FOREIGN KEY ([JoinedSessions_Id])
    REFERENCES [dbo].[Sessions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GymMemberSession1_Session'
CREATE INDEX [IX_FK_GymMemberSession1_Session]
ON [dbo].[GymMemberSession1]
    ([JoinedSessions_Id]);
GO

-- Creating foreign key on [GymMemberId] in table 'SessionFeedbacks'
ALTER TABLE [dbo].[SessionFeedbacks]
ADD CONSTRAINT [FK_GymMemberSessionFeedback]
    FOREIGN KEY ([GymMemberId])
    REFERENCES [dbo].[GymMembers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GymMemberSessionFeedback'
CREATE INDEX [IX_FK_GymMemberSessionFeedback]
ON [dbo].[SessionFeedbacks]
    ([GymMemberId]);
GO

-- Creating foreign key on [SessionId] in table 'SessionFeedbacks'
ALTER TABLE [dbo].[SessionFeedbacks]
ADD CONSTRAINT [FK_SessionSessionFeedback]
    FOREIGN KEY ([SessionId])
    REFERENCES [dbo].[Sessions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SessionSessionFeedback'
CREATE INDEX [IX_FK_SessionSessionFeedback]
ON [dbo].[SessionFeedbacks]
    ([SessionId]);
GO

-- Creating foreign key on [TrainerId] in table 'TrainerPersonalAppointmentSessions'
ALTER TABLE [dbo].[TrainerPersonalAppointmentSessions]
ADD CONSTRAINT [FK_GymMemberTrainerPersonalAppointmentSession]
    FOREIGN KEY ([TrainerId])
    REFERENCES [dbo].[GymMembers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GymMemberTrainerPersonalAppointmentSession'
CREATE INDEX [IX_FK_GymMemberTrainerPersonalAppointmentSession]
ON [dbo].[TrainerPersonalAppointmentSessions]
    ([TrainerId]);
GO

-- Creating foreign key on [GymMemberId] in table 'PersonalAppointmentRequests1'
ALTER TABLE [dbo].[PersonalAppointmentRequests1]
ADD CONSTRAINT [FK_GymMemberPersonalAppointmentRequest]
    FOREIGN KEY ([GymMemberId])
    REFERENCES [dbo].[GymMembers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GymMemberPersonalAppointmentRequest'
CREATE INDEX [IX_FK_GymMemberPersonalAppointmentRequest]
ON [dbo].[PersonalAppointmentRequests1]
    ([GymMemberId]);
GO

-- Creating foreign key on [TrainerPersonalAppointmentSession_Id] in table 'PersonalAppointmentRequests1'
ALTER TABLE [dbo].[PersonalAppointmentRequests1]
ADD CONSTRAINT [FK_PersonalAppointmentRequestTrainerPersonalAppointmentSession]
    FOREIGN KEY ([TrainerPersonalAppointmentSession_Id])
    REFERENCES [dbo].[TrainerPersonalAppointmentSessions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonalAppointmentRequestTrainerPersonalAppointmentSession'
CREATE INDEX [IX_FK_PersonalAppointmentRequestTrainerPersonalAppointmentSession]
ON [dbo].[PersonalAppointmentRequests1]
    ([TrainerPersonalAppointmentSession_Id]);
GO

-- Creating foreign key on [Owner] in table 'PartnerGroups'
ALTER TABLE [dbo].[PartnerGroups]
ADD CONSTRAINT [FK_GymMemberPartnerGroup]
    FOREIGN KEY ([Owner])
    REFERENCES [dbo].[GymMembers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GymMemberPartnerGroup'
CREATE INDEX [IX_FK_GymMemberPartnerGroup]
ON [dbo].[PartnerGroups]
    ([Owner]);
GO

-- Creating foreign key on [PartnerGroupId] in table 'GroupJoinRequests'
ALTER TABLE [dbo].[GroupJoinRequests]
ADD CONSTRAINT [FK_PartnerGroupGroupJoinRequest]
    FOREIGN KEY ([PartnerGroupId])
    REFERENCES [dbo].[PartnerGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PartnerGroupGroupJoinRequest'
CREATE INDEX [IX_FK_PartnerGroupGroupJoinRequest]
ON [dbo].[GroupJoinRequests]
    ([PartnerGroupId]);
GO

-- Creating foreign key on [GymMemberId] in table 'GroupJoinRequests'
ALTER TABLE [dbo].[GroupJoinRequests]
ADD CONSTRAINT [FK_GymMemberGroupJoinRequest]
    FOREIGN KEY ([GymMemberId])
    REFERENCES [dbo].[GymMembers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GymMemberGroupJoinRequest'
CREATE INDEX [IX_FK_GymMemberGroupJoinRequest]
ON [dbo].[GroupJoinRequests]
    ([GymMemberId]);
GO

-- Creating foreign key on [GymMembers_Id] in table 'GymMembersInPartnerGroup'
ALTER TABLE [dbo].[GymMembersInPartnerGroup]
ADD CONSTRAINT [FK_GymMembersInPartnerGroup_GymMember]
    FOREIGN KEY ([GymMembers_Id])
    REFERENCES [dbo].[GymMembers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [JoinedGroups_Id] in table 'GymMembersInPartnerGroup'
ALTER TABLE [dbo].[GymMembersInPartnerGroup]
ADD CONSTRAINT [FK_GymMembersInPartnerGroup_PartnerGroup]
    FOREIGN KEY ([JoinedGroups_Id])
    REFERENCES [dbo].[PartnerGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GymMembersInPartnerGroup_PartnerGroup'
CREATE INDEX [IX_FK_GymMembersInPartnerGroup_PartnerGroup]
ON [dbo].[GymMembersInPartnerGroup]
    ([JoinedGroups_Id]);
GO

-- Creating foreign key on [PartnerGroupId] in table 'GroupActivities'
ALTER TABLE [dbo].[GroupActivities]
ADD CONSTRAINT [FK_PartnerGroupGroupActivity]
    FOREIGN KEY ([PartnerGroupId])
    REFERENCES [dbo].[PartnerGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PartnerGroupGroupActivity'
CREATE INDEX [IX_FK_PartnerGroupGroupActivity]
ON [dbo].[GroupActivities]
    ([PartnerGroupId]);
GO

-- Creating foreign key on [GymMembers_Id] in table 'GymMemberGroupActivity'
ALTER TABLE [dbo].[GymMemberGroupActivity]
ADD CONSTRAINT [FK_GymMemberGroupActivity_GymMember]
    FOREIGN KEY ([GymMembers_Id])
    REFERENCES [dbo].[GymMembers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [GroupActivities_Id] in table 'GymMemberGroupActivity'
ALTER TABLE [dbo].[GymMemberGroupActivity]
ADD CONSTRAINT [FK_GymMemberGroupActivity_GroupActivity]
    FOREIGN KEY ([GroupActivities_Id])
    REFERENCES [dbo].[GroupActivities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GymMemberGroupActivity_GroupActivity'
CREATE INDEX [IX_FK_GymMemberGroupActivity_GroupActivity]
ON [dbo].[GymMemberGroupActivity]
    ([GroupActivities_Id]);
GO

-- Creating foreign key on [NotificationType_Id] in table 'Notifications'
ALTER TABLE [dbo].[Notifications]
ADD CONSTRAINT [FK_NotificationNotificationType]
    FOREIGN KEY ([NotificationType_Id])
    REFERENCES [dbo].[NotificationTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NotificationNotificationType'
CREATE INDEX [IX_FK_NotificationNotificationType]
ON [dbo].[Notifications]
    ([NotificationType_Id]);
GO

-- Creating foreign key on [GymMemberId] in table 'Notifications'
ALTER TABLE [dbo].[Notifications]
ADD CONSTRAINT [FK_GymMemberNotification]
    FOREIGN KEY ([GymMemberId])
    REFERENCES [dbo].[GymMembers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GymMemberNotification'
CREATE INDEX [IX_FK_GymMemberNotification]
ON [dbo].[Notifications]
    ([GymMemberId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------