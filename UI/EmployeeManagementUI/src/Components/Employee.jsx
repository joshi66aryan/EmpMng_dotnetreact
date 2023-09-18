import React, {useState, useEffect, useRef} from 'react';
import placeholderImg from '../assets/placeholderImg.png'



import {
  MDBContainer,
  MDBInput,
  MDBBtn, 
  MDBCard,
  MDBCardBody,
  MDBCardTitle,
  MDBCardImage

} from 'mdb-react-ui-kit';



const Employee = ({ addOrEdit, edit, setEdit, selectedUser }) => {

  const fileInputRef = useRef(null);
  
  const initialFieldsValues = {
    employeeid: 0,
    employeeName:'',
    occupation:'',
    imageName:'',
    imageSrc: placeholderImg,
    imageFile: null
  }
  const [values, setValues] = useState(initialFieldsValues)
  const { employeeid, employeeName, occupation, imageName, imageSrc, imageFile } = values //destructuring values

  const [errors, setErrors] = useState({
    nameError: '',
    occupationError: ''
  });

  const { nameError, occupationError } = errors   // destructuring error property.

  //  populating values, which in turn populate all the 'value' and display the data of user which is choose for edit.
  useEffect(() => {

    if( selectedUser != null) {
      setValues(selectedUser)
    }
  }, [selectedUser])
  


  // handle the change in input field.
  const handleInputChange = (e) => { 

    const {name, value} = e.target;
    setValues({
      ...values,
      [name]:value
    })
  }

  // show the preview of uploded picture.
  const showPreview = (e) => {

    if(e.target.files && e.target.files[0]) {

        let selectedImageFile = e.target.files[0];   
        const reader = new FileReader();

        reader.onload = x => {     // updating properties of values state
          setValues({
            ...values,
            imageFile: selectedImageFile,       
            imageSrc: x.target.result
          }) 
        }
        reader.readAsDataURL(selectedImageFile)
    } 
    else {

        setValues({   // if image isn't uploade, then show placeholder image.
          ...values,
          imageFile: null, 
          imageSrc: placeholderImg,
          imageName: ''
        })
    }
  }

  // to submit the form data to backend server. // it is send to parent component --> App.jsx
  const handleSubmit = (e) => {

    e.preventDefault();
    
    if(!occupation.length){
        setErrors({ occupationError: 'Enter your occupation!' })
    }

    if(!employeeName.length){
      setErrors({ nameError: 'Enter your name!' })
    }

    if (edit) {
      if(employeeName  && occupation ){ 

      const formData = new FormData()
      formData.append('employeeid', employeeid)
      formData.append('employeeName',employeeName)
      formData.append('occupation',occupation)
      formData.append('imageName',imageName)
      formData.append('imageFile',imageFile)
      
      addOrEdit(formData, resetForm)
      }

    } else {

      if(employeeName && imageFile && occupation ){

        const formData = new FormData()
        formData.append('employeeid', employeeid)
        formData.append('employeeName',employeeName)
        formData.append('occupation',occupation)
        formData.append('imageName',imageName)
        formData.append('imageFile',imageFile)
        
        addOrEdit(formData, resetForm)
      } 
    }

  }

  const resetForm = () => {
    setValues(initialFieldsValues)
    fileInputRef.current.value = null;

  } 

  return (
    
    <MDBContainer className="p-3 my-5 d-flex flex-column w-50">

        <MDBCard>
          <MDBCardBody>

            <div style = {{
              display: 'flex',
              justifyContent: 'center',
              alignItems: 'center'
            }}>

              <div style = {{
                  maxWidth:'450px',
                  display: 'flex',
                  justifyContent: 'center', // Center the image horizontally
                  alignItems: 'center'
                }}>

                <MDBCardImage 
                  src= {imageSrc} 
                  alt="Card image"  
                  style={{ 
                      width:'100%',
                    }}
                />

              </div>
            </div>
  

            <MDBCardTitle style={{ marginTop:'40px'}}> Enter Your Details </MDBCardTitle>
            <form onSubmit = {handleSubmit}>

              <MDBInput 
                wrapperClass='mb-4' 
                type='file'
                accept='image/*'
                onChange={showPreview}
                ref={fileInputRef}
              />

              <MDBInput
                wrapperClass='mb-2' 
                label='Name' 
                type='text' 
                name="employeeName" 
                value={employeeName}
                onChange = { handleInputChange }
              />
              { nameError && <div className=" " style={{color : "#f93154", marginBottom:'20px'}}>{nameError}</div>}

              <MDBInput 
                wrapperClass='mb-2' 
                label='Occupation' 
                type='text'
                name="occupation" 
                value={occupation}
                onChange = { handleInputChange }
              />
              {occupationError && <div className=" " style={{color : "#f93154", marginBottom:'20px'}}>{occupationError}</div>}

              <MDBBtn className="mb-4" type ='submit'>Submit</MDBBtn>

              {
                edit? <MDBBtn 
                        className="mb-4" 
                        type ='submit' 
                        style={{ marginLeft: "10px" }} 
                        color="danger"
                        onClick = {() => { 
                          setEdit(false);
                          setValues(initialFieldsValues);

                        }}
                      >
                        Cancel  
                      </MDBBtn>: null
              }
            </form>
          </MDBCardBody>
        </MDBCard>
    </MDBContainer>
  )


}

export default Employee








