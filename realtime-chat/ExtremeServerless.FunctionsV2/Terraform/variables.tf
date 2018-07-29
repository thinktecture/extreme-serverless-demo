variable "location" {
  type = "string"
}

variable "resource_group_name" {
  type = "string"
}

variable "storage_account_name" {
  type = "string"
}

variable "appservice_plan_name" {
  type = "string"
}

variable "functionapp_name" {
  type = "string"
}

variable "cosmosdb_account_name" {
  type = "string"
}

variable "tags" {
  type = "map"

  default = {
    Environment = "Demo"
    Responsible = "Christian Weyer"
  }
}
