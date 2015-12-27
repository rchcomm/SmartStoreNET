namespace SmartStore.Data.Migrations
{
	using System.Data.Entity.Migrations;
	using Core.Domain.Customers;
	using Core.Domain.Security;
	using Setup;

	public partial class ImportFramework : DbMigration, ILocaleResourcesProvider, IDataSeeder<SmartObjectContext>
	{
        public override void Up()
        {
            CreateTable(
                "dbo.ImportProfile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        FolderName = c.String(nullable: false, maxLength: 100),
                        FileTypeId = c.Int(nullable: false),
                        EntityTypeId = c.Int(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        Skip = c.Int(nullable: false),
                        Take = c.Int(nullable: false),
                        FileTypeConfiguration = c.String(),
                        ColumnMapping = c.String(),
                        SchedulingTaskId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ScheduleTask", t => t.SchedulingTaskId)
                .Index(t => t.SchedulingTaskId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImportProfile", "SchedulingTaskId", "dbo.ScheduleTask");
            DropIndex("dbo.ImportProfile", new[] { "SchedulingTaskId" });
            DropTable("dbo.ImportProfile");
        }

		public bool RollbackOnFailure
		{
			get { return false; }
		}

		public void Seed(SmartObjectContext context)
		{
			context.MigrateLocaleResources(MigrateLocaleResources);

			var permissionMigrator = new PermissionMigrator(context);
			var activityLogMigrator = new ActivityLogTypeMigrator(context);

			permissionMigrator.AddPermission(new PermissionRecord
			{
				Name = "Admin area. Manage Imports",
				SystemName = "ManageImports",
				Category = "Configuration"
			}, new string[] { SystemCustomerRoleNames.Administrators });

			activityLogMigrator.AddActivityLogType("DeleteOrder", "Delete order", "Auftrag gel�scht");
		}

		public void MigrateLocaleResources(LocaleResourcesBuilder builder)
		{
			builder.AddOrUpdate("Admin.Common.RecordsSkip",
				"Skip",
				"�berspringen",
				"Specifies the number of records to be skipped.",
				"Legt die Anzahl der zu �berspringenden Datens�tze fest.");

			builder.AddOrUpdate("Common.Unknown", "Unknown", "Unbekannt");
			builder.AddOrUpdate("Common.Language", "Language", "Sprache");
			builder.AddOrUpdate("Admin.Common.ImportFile", "Import file", "Importdatei");
			builder.AddOrUpdate("Admin.Common.ImportFiles", "Import files", "Importdateien");
			builder.AddOrUpdate("Admin.Common.CsvConfiguration", "CSV Configuration", "CSV Konfiguration");

			builder.AddOrUpdate("Admin.Common.RecordsTake",
				"Limit",
				"Begrenzen",
				"Specifies the maximum number of records to be processed.",
				"Legt die maximale Anzahl der zu verarbeitenden Datens�tze fest.");

			builder.AddOrUpdate("Admin.Common.FileTypeMustEqual",
				"The file must be of the type {0}.",
				"Die Datei muss vom Typ {0} sein.");

			builder.AddOrUpdate("Admin.DataExchange.Import.NoProfiles",
				"There were no import profiles found.",
				"Es wurden keine Importprofile gefunden.");

			builder.AddOrUpdate("Admin.DataExchange.Import.Name",
				"Name of profile",
				"Name des Profils",
				"Specifies the name of the import profile.",
				"Legt den Namen des Importprofils fest.");

			builder.AddOrUpdate("Admin.DataExchange.Import.FileType",
				"File type",
				"Dateityp",
				"The file type of the import file(s).",
				"Der Dateityp der Importdatei(en).");

			builder.AddOrUpdate("Admin.DataExchange.Import.ProgressInfo",
				"{0} of {1} records processed",
				"{0} von {1} Datens�tzen verarbeitet");


			builder.AddOrUpdate("Enums.SmartStore.Core.Domain.DataExchange.ImportEntityType.Product", "Product", "Produkt");
			builder.AddOrUpdate("Enums.SmartStore.Core.Domain.DataExchange.ImportEntityType.Customer", "Customer", "Kunde");
			builder.AddOrUpdate("Enums.SmartStore.Core.Domain.DataExchange.ImportEntityType.NewsLetterSubscription", "Newsletter Subscriber", "Newsletter Abonnent");
			builder.AddOrUpdate("Enums.SmartStore.Core.Domain.DataExchange.ImportEntityType.Category", "Category", "Warengruppe");

			builder.AddOrUpdate("Enums.SmartStore.Core.Domain.DataExchange.ImportFileType.CSV", "Delimiter separated values (.csv)", "Trennzeichen getrennte Werte (.csv)");
			builder.AddOrUpdate("Enums.SmartStore.Core.Domain.DataExchange.ImportFileType.XLSX", "Excel (.xlsx)", "Excel  (.xlsx)");

			builder.AddOrUpdate("Admin.DataExchange.Import.FileUpload",
				"Upload import file...",
				"Importdatei hochladen...");

			builder.AddOrUpdate("Admin.DataExchange.Import.MissingImportFile",
				"Please upload an import file.",
				"Bitte laden Sie eine Importdatei hoch.");

			builder.AddOrUpdate("Admin.DataExchange.Csv.QuoteAllFields",
				"Quote all fields",
				"Alle Felder in Anf�hrungszeichen",
				"Specifies whether to set quotation marks around all field values.",
				"Legt fest, ob die Werte aller Felder in Anf�hrungszeichen gestellt werden sollen.");

			builder.AddOrUpdate("Admin.DataExchange.Csv.TrimValues",
				"Trim values",
				"�berfl�ssige Leerzeichen entfernen",
				"Specifies whether to remove space characters at start and end of a field value.",
				"Legt fest, ob Leerzeichen am Anfang und am Ende eines Feldwertes entfernt werden sollen.");

			builder.AddOrUpdate("Admin.DataExchange.Csv.SupportsMultiline",
				"Supports multilines",
				"Mehrzeilen erlaubt",
				"Specifies whether field values with multilines are supported.",
				"Legt fest, ob mehrzeilige Feldwerte unterst�tzt werden.");

			builder.AddOrUpdate("Admin.DataExchange.Csv.Delimiter",
				"Delimiter",
				"Trennzeichen",
				"Specifies the field separator.",
				"Legt das zu verwendende Trennzeichen f�r die Felder fest.");

			builder.AddOrUpdate("Admin.DataExchange.Csv.Quote",
				"Quote character",
				"Anf�hrungszeichen",
				"Spacifies the quotation character.",
				"Legt das zu verwendende Anf�hrungszeichen fest.");

			builder.AddOrUpdate("Admin.DataExchange.Csv.Escape",
				"Inner quote character",
				"Inneres Anf�hrungszeichen",
				"Specifies the inner quote character used for escaping.",
				"Legt das innere Anf�hrungszeichen (Escaping) fest.");

			builder.AddOrUpdate("Admin.DataExchange.Csv.Delimiter.Validation",
				"Please enter a valid delimiter.",
				"Geben Sie bitte ein g�ltiges Trennzeichen ein.");

			builder.AddOrUpdate("Admin.DataExchange.Csv.Quote.Validation",
				"Please enter a valid quote character.",
				"Geben Sie bitte ein g�ltiges Anf�hrungszeichen ein.");

			builder.AddOrUpdate("Admin.DataExchange.Csv.Escape.Validation",
				"Please enter a valid inner quote character (escaping).",
				"Geben Sie bitte ein g�ltiges, inneres Anf�hrungszeichen (Escaping) ein.");

			builder.AddOrUpdate("Admin.DataExchange.Csv.EscapeDelimiter.Validation",
				"Delimiter and inner quote character cannot be equal in CSV files.",
				"Trennzeichen und inneres Anf�hrungszeichen k�nnen in CSV Dateien nicht gleich sein.");

			builder.AddOrUpdate("Admin.DataExchange.Csv.QuoteDelimiter.Validation",
				"Delimiter and quote character cannot be equal in CSV files.",
				"Trennzeichen und Anf�hrungszeichen k�nnen in CSV Dateien nicht gleich sein.");


			builder.AddOrUpdate("Admin.Catalog.Products.Fields.BasePriceMeasureUnit", "Base price measure unit", "Grundpreis Ma�einheit");
			builder.AddOrUpdate("Admin.Catalog.Products.Fields.ApprovedRatingSum", "Approved rating sum", "Summe genehmigter Bewertungen");
			builder.AddOrUpdate("Admin.Catalog.Products.Fields.NotApprovedRatingSum", "Not approved rating sum", "Summe nicht genehmigter Bewertungen");
			builder.AddOrUpdate("Admin.Catalog.Products.Fields.ApprovedTotalReviews", "Approved total reviews", "Summe genehmigter Rezensionen");
			builder.AddOrUpdate("Admin.Catalog.Products.Fields.NotApprovedTotalReviews", "Not approved total reviews", "Summe nicht genehmigter Rezensionen");
			builder.AddOrUpdate("Admin.Catalog.Products.Fields.HasTierPrices", "Has tier prices", "Hat Staffelpreise");
			builder.AddOrUpdate("Admin.Catalog.Products.Fields.LowestAttributeCombinationPrice", "Lowest attribute combination price", "Niedrigster Attributkombinationspreis");
			builder.AddOrUpdate("Admin.Catalog.Products.Fields.HasDiscountsApplied", "Has discounts applied", "Hat angewendete Rabatte");

			builder.AddOrUpdate("Admin.Catalog.Categories.Fields.ParentCategory", "Parent category", "�bergeordnete Warengruppe");

			builder.AddOrUpdate("Admin.Customers.Customers.Fields.CustomerGuid", "Customer GUID", "Kunden GUID");
			builder.AddOrUpdate("Admin.Customers.Customers.Fields.PasswordSalt", "Password salt", "Passwort Salt");
			builder.AddOrUpdate("Admin.Customers.Customers.Fields.IsSystemAccount", "Is system account", "Ist Systemkonto");
			builder.AddOrUpdate("Admin.Customers.Customers.Fields.LastLoginDateUtc", "Last login date", "Letztes Login-Datum");

			builder.AddOrUpdate("Admin.Promotions.NewsLetterSubscriptions.Fields.NewsLetterSubscriptionGuid", "Subscription GUID", "Abonnement GUID");

			builder.AddOrUpdate("Admin.DataExchange.ColumnMapping.Note",
				"For each field of the import file you can optionally set whether and to which entity property the data is to be imported. Specifying a default value is also possible, which is applied when the import field is empty. Through <b>Reset</b> all made assignments are reset to their original values.",
				"Sie k�nnen optional f�r jedes Feld der Importdatei festlegen, ob und nach welcher Entit�tseigenschaft dessen Daten importiert werden sollen. Zudem ist die Angabe eines Standardwertes m�glich, der angewendet wird, wenn das Importfeld leer ist. �ber <b>Zur�cksetzen</b> werden alle get�tigten Zuordnungen auf ihre Ursprungswerte zur�ckgesetzt.");

			builder.AddOrUpdate("Admin.DataExchange.ColumnMapping.ImportField", "Import Field", "Importfeld");
			builder.AddOrUpdate("Admin.DataExchange.ColumnMapping.EntityProperty", "Entity property", "Eigenschaft der Entit�t");
			builder.AddOrUpdate("Admin.DataExchange.ColumnMapping.DefaultValue", "Default Value", "Standardwert");

			builder.AddOrUpdate("Admin.DataExchange.ColumnMapping.Validate.EntityMultipleMapped",
				"The entity property \"{0}\" was assigned several times. Please assign each property only once.",
				"Die Entit�tseigenschaft \"{0}\" wurde mehrfach zugeodnet. Bitte ordnen Sie jede Eigenschaft nur einmal zu.");


			builder.Delete(
				"Admin.DataExchange.Export.LastExecution",
				"Admin.DataExchange.Export.Offset",
				"Admin.DataExchange.Export.Limit",
				"Admin.Promotions.NewsLetterSubscriptions.ImportEmailsSuccess",
				"Admin.Common.ImportFromCsv",
				"Admin.Common.CsvFile",

				"Admin.Common.ImportFromExcel",
				"Admin.Common.ExcelFile",
				"Admin.Common.ImportFromExcel.InProgress",
				"Admin.Common.ImportFromExcel.LastResultTitle",
				"Admin.Common.ImportFromExcel.ProcessedCount",
				"Admin.Common.ImportFromExcel.QuickStats",
				"Admin.Common.ImportFromExcel.ActiveSince",
				"Admin.Common.ImportFromExcel.CancelPrompt",
				"Admin.Common.ImportFromExcel.Cancel",
				"Admin.Common.ImportFromExcel.Cancelled",
				"Admin.Common.ImportFromExcel.DownloadReport",
				"Admin.Common.ImportFromExcel.NoReportAvailable"
			);

			builder.AddOrUpdate("ActivityLog.DeleteOrder", "Deleted order {0}", "Auftrag {0} gel�scht");
		}
	}
}