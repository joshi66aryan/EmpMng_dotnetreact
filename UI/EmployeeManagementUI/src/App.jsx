import react, {useState ,useEffect} from 'react';
import EmployeeList from "./Components/EmployeeList"
import Employee from "./Components/Employee"
import Navbar from "./Components/Navbar"
import { Routes, Route} from 'react-router-dom'
import axios from "axios"

function App() {

  const [employeeList, setEmployeeList] = useState([])
  const [edit, setEdit] = useState(false)
  const [selectedUser, setSelectedUser] = useState()

  const url ='https://localhost:7225/api/EmployeeAuth/'


  // fetch from backend as soon as app load.
  useEffect(() => {
    refreshEmployeeList();
  }, [ ])

  // listing all the api.
  const fetchEmployee = () => {

    const headers = {
      "Content-Type": "multipart/form-data" // Set the correct content type
    };

    return {
      fetchAll: () => axios.get(url),
      create: newRecord => axios.post(url, newRecord, { headers }),
      update: (id, updateRecord) => axios.put(url + id, updateRecord, { headers }),
      delete: id => axios.delete(url + id)
    };
  }

  // fetch from backend.
  const refreshEmployeeList = () => {
    fetchEmployee().fetchAll()
    .then(res => setEmployeeList(res.data))
    .catch(err => console.log(err))
  }



  // submit on backend.
  const addOrEdit = (formData, onSuccess) => {

    if( formData.get('employeeid' ) == "0"){    /// if employee id is 0 it is create operation, otherwise it is update operation.

      fetchEmployee().create(formData)
      .then(res => {
        onSuccess();
        refreshEmployeeList();
      })
      .catch(err => console.log(err))

    }
    else {

      fetchEmployee().update(formData.get('employeeid'), formData)
      .then(res => {
        onSuccess();
        refreshEmployeeList();
      })
      .catch(err => console.log(err))

    }
  }

  // selecting user  by id from state employeeList after clicking on edit.
  const getUserById = (x) => {
      setSelectedUser( employeeList.find(user => user.employeeid === x));
  }

  const onDelete = (id) => {
    if(window.confirm('Are you sure to delete this record?')){
      fetchEmployee().delete(id)
      .then(res => refreshEmployeeList())
      .catch(err => console.log(err)) 
    }
  }



  return (
    <div>
      <Navbar/> 
      <Routes>

        <Route 
          path ='/form' 
          element = {
            
            <Employee 
              addOrEdit={addOrEdit} 
              edit = {edit} 
              setEdit = {setEdit} 
              selectedUser = {selectedUser}  
            />
          }
        />

        <Route 
          path = '/display' 
          element = { 
            
            <EmployeeList  
              employeeList = {employeeList} 
              setEdit = { setEdit} 
              getUserById = {getUserById} 
              onDelete = {onDelete}
            />
          }
        />
      </Routes>
    </div>

  )
}

export default App
