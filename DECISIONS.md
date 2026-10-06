# Decisions
- Use the business name (capitalized and with removed suffixes) to identify dealers as this is the only field present across all sources
- Use `System.Text.Json` for deserializing JSON data sources
- Use `CsvHelper` library for deserializing CSV data sources
- Create an `IDealerImporter` interface with implementations for each data source which isolates the parsing and population of data for different file formats
- Store intermediate values in a `Dictionary` lookup keyed by the business names during the import
- Design each importer to add field values to an object that contains all possible properties and will be consolidated with all data sources at the end of the import
- Assign a hard-coded priority to each IDealerImporter implementation, based on how reliable the data source is, to make sure more reliable data is used first, with less reliable sources only being used to fill remaining gaps
- Perform a database lookup during consolidation for existing dealers to make sure duplicate dealers are not inserted into the database

### Due to time constraints, the following has not been implemented:
- Format handlers for the VAT lookups of SAF membership files were not implemented; however, the architecture is extensible, so new `IDealerImporter` classes can be added with minimal changes to the core execution loop
- Normalization of address fields has not been implemented meaning the data on some dealers is in a different format depending on the source
- Some crawled websites do not have a value in the `business_name_detected`, and in the current implementation are ignored. This should be changed to use other fields besides the business name to match the data with other sources
