module MyDataGrid.DataGrid

open Elmish
open Elmish.WPF
open System

type Visit = 
    {   ServiceTime: DateTime option
        DoNotSee: Boolean option
        ChartNumber: int option
        LastName: string option
        FirstName: string option
        Mi: string option
        BirthDate: DateTime option
        PostingTime: DateTime option 
        AppointmentTime: DateTime option }

type Cell = 
  {RowNumber: int
   ColumnNumber: int
   AppointmentKeys: Visit list
   ColumnTime: TimeSpan
   AppointmentCount: int
   AppointmentTime: DateTime option  // all lines in the cell have the same appointment time.     
  }

let SetCell (rowNumber: int, columnNumber: int) =
    let AppointmentsPerCell = 4
    {RowNumber = rowNumber
     ColumnNumber = columnNumber
     AppointmentKeys = [for x in 1 .. AppointmentsPerCell -> 
                           {
                             ServiceTime = Some System.DateTime.Now 
                             DoNotSee = Some false 
                             ChartNumber = Some 8812 
                             LastName= Some ("LastName" + string x)
                             FirstName= Some ("FirstName" + string x)
                             Mi = Some "J" 
                             BirthDate = Some(DateTime(2020,09,14))
                             PostingTime = Some DateTime.Now
                             AppointmentTime = Some DateTime.Now
                         }]      
     ColumnTime = System.TimeSpan.FromMinutes(float(columnNumber * 15))
     AppointmentCount = 4
     AppointmentTime = Some(DateTime.Now)
     }

type Row =
  {RowTime: string
   Columns: Cell list}

let SetRow (rowNumber: int, startTime: System.TimeSpan)= 
    let columnCount = 4
    let hr = System.TimeSpan.FromHours(1.0)
    let rowTime = startTime + System.TimeSpan.FromTicks(hr.Ticks * int64(rowNumber))
    { RowTime = rowTime.ToString("h':00'")
      Columns = [for columnNumber in 1 .. columnCount -> SetCell(rowNumber, columnNumber) ]
    }

type Model =
  { AppointmentDate: DateTime
    Rows: Row list
    SelectedRow: Row option}

type Msg =
  | SetAppointmentDate of DateTime
  | SetSelectedRow of Row option

let init =
      let rowCount = 9
      let startTime = TimeSpan.FromHours(float(8))
      { AppointmentDate = DateTime.Now 
        Rows = [for rowNumber in 0 .. rowCount -> SetRow(rowNumber, startTime)]
        SelectedRow = None
      }

let update msg m =
  match msg with
  | SetAppointmentDate d -> {m with AppointmentDate = d}
  | SetSelectedRow r -> {m with SelectedRow = r}

let bindings () : Binding<Model, Msg> list = [
  "SelectedAppointmentDate" |> Binding.twoWay( (fun m -> m.AppointmentDate), SetAppointmentDate)
  "Rows" |> Binding.oneWay( fun m -> m.Rows)
  "SelectedRow" |> Binding.twoWay( (fun m -> m.SelectedRow), SetSelectedRow)
]

let designVm = ViewModel.designInstance init (bindings ())


let main window =
  Program.mkSimpleWpf (fun () -> init) update bindings
  |> Program.withConsoleTrace
  |> Program.runWindowWithConfig
    { ElmConfig.Default with LogConsole = true; Measure = true }
    window
