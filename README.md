# MyDataGrid

This is my first attempt at a date in an appointment book. The logic is that a DataGrid consists of "Rows" -- a list of 10 Row types (always). 
Each Row then consists of a list of 4 Cells -- one cell per column (always). Each cell is displayed in a ListView and consists of 0 to 4 possible 
appointment names (i.e., Visit). Each Row and Cell is assigned a time on the specified date. (All appointments within a cell have the same time).

My initial problem of using an index [] in xaml of: "{Binding Columns[0].AppointmentKeys}" seems to have been resolved when 
recreating the problem in for this demo. 

However, I still have some major glaring problems:
  FirstName should be "FirstName1"  and not "Some(FirstName1)"
  LastName should be "LastName1"    and not "LastName1"
  BirthDate should be "09/14/2020"  and not "Some(09/14/2020 00:00:00)"
  
  Also, I would have thought that the binding to Rows should be a Binding.subModel, so even though "Rows" |> Binding.oneWay( fun m -> m.Rows) 
  seems to work, is it the best way to do this?
  
  Lastly, at some point the appointment names will be edited/added/deleted etc (say from a database) -- will this update the UI?
  
  Thanks for any thoughts.
