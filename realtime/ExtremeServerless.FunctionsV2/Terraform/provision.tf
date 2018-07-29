provider "azurerm" {}

resource "azurerm_resource_group" "serverless" {
  name     = "${var.resource_group_name}"
  location = "${var.location}"
  tags     = "${var.tags}"
}

resource "azurerm_storage_account" "serverless" {
  name                     = "${var.storage_account_name}"
  resource_group_name      = "${azurerm_resource_group.serverless.name}"
  location                 = "${azurerm_resource_group.serverless.location}"
  account_tier             = "Standard"
  account_replication_type = "LRS"
  tags                     = "${var.tags}"
}

resource "azurerm_app_service_plan" "serverless" {
  name                = "${var.appservice_plan_name}"
  location            = "${azurerm_resource_group.serverless.location}"
  resource_group_name = "${azurerm_resource_group.serverless.name}"
  kind                = "FunctionApp"
  tags                = "${var.tags}"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_function_app" "serverless" {
  name                      = "${var.functionapp_name}"
  location                  = "${azurerm_resource_group.serverless.location}"
  resource_group_name       = "${azurerm_resource_group.serverless.name}"
  app_service_plan_id       = "${azurerm_app_service_plan.serverless.id}"
  storage_connection_string = "${azurerm_storage_account.serverless.primary_connection_string}"
  version                   = "beta"
  tags                      = "${var.tags}"
}

resource "azurerm_cosmosdb_account" "serverless" {
  name                = "${var.cosmosdb_account_name}"
  location            = "${azurerm_resource_group.serverless.location}"
  resource_group_name = "${azurerm_resource_group.serverless.name}"
  offer_type          = "Standard"
  kind                = "GlobalDocumentDB"

  enable_automatic_failover = true

  consistency_policy {
    consistency_level = "Session"
  }

  geo_location {
    location          = "${azurerm_resource_group.serverless.location}"
    failover_priority = 0
  }
}

# Creating Cosmos DB collections is not yet supported... :-/

