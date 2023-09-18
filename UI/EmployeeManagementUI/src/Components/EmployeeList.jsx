import React from 'react'
import {
  MDBCard,
  MDBCardBody,
  MDBContainer,
  MDBBtn,
  MDBTable,
  MDBTableHead,
  MDBTableBody
} from 'mdb-react-ui-kit';

import { Link } from 'react-router-dom';

const EmployeeList = ({ employeeList, setEdit, getUserById, onDelete }) => {

  const showImg = img => <img 
                            src = {img} 
                            className='card-img-top rounded-circle'   
                            style={{ 
                              width: "50px", 
                              height: "50px", 
                              borderRadius: "50%"
                            }}
                          />


  return (
    <MDBContainer className=" p-3 my-5 d-flex flex-column ">
      <MDBCard>
        <MDBCardBody>
          <MDBTable>
            <MDBTableHead>

            <tr>
              <th scope='col'>Id</th>
              <th scope='col'>Name</th>
              <th scope='col'>Occupation</th>
              <th scope='col'>Image</th>
              <th scope='col'>Action</th>
            </tr>
          
            </MDBTableHead>

            <MDBTableBody>

              {
                employeeList
                  .map((value, i) => 
                    <tr key ={i} >
                      <th scope='row'>{value.employeeid}</th>
                      <td> { value.employeeName}</td>
                      <td> { value. occupation }</td>
                      <td> { showImg(value.imageSrc) }</td>
                      <td>
                        <Link to ='/form' >
                          <MDBBtn 
                                color='link' 
                                rounded 
                                size='sm' 
                                onClick = {() => { 
                                      setEdit(true); 
                                      getUserById(value.employeeid); 
                                    }} 
                          >
                            Edit
                          </MDBBtn>
                        </Link>


                        <MDBBtn 
                          color='link' 
                          rounded 
                          size='sm' 
                          onClick = {(e) => {onDelete(value.employeeid)}} 
                          >
                            Delete
                          </MDBBtn>
                      </td>
                    </tr>
                 )   
              }

            </MDBTableBody>

          </MDBTable>
        </MDBCardBody>
      </MDBCard>
    </MDBContainer>
  )
}

export default EmployeeList












    