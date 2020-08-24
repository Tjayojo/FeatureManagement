import React from 'react';
import { bool, shape, func, string } from 'prop-types';
import {
  Modal,
  ModalHeader,
  ModalBody,
  Button,
  Form,
  FormGroup,
  Label,
  Input,
  FormFeedback
} from 'reactstrap';
import { Formik } from 'formik';
import * as Yup from 'yup';

const EditFeatureModal = ({ feature, showModal, saveFeature, toggleModal }) => {
  const onSaveFeature = newFeature => {
    saveFeature(newFeature)
      .then(() => {
        toggleModal();
      });
  };

  const title = !feature || !feature.id ? 'Create Feature' : `Edit ${feature.name}`;
  const initialValue = feature || { name: '', description: '' };
  const newFeatureSchema = Yup.object()
    .shape({
      name: Yup.string()
        .min(5, 'Name must be at least 5 characters')
        .max(50, 'Name cannot be longer than 50 characters')
        .required('Feature name is required'),
      description: Yup.string()
        .max(250, 'Description cannot be greater than 250 characters')
        .nullable()
    });
  const handleSubmit = (values, { setSubmitting }) => {
    onSaveFeature(values);
    setSubmitting(false);
  };
  return (
    <>
      <Modal isOpen={showModal} toggle={toggleModal}>
        <ModalHeader toggle={toggleModal}>{title}</ModalHeader>
        <ModalBody>
          <Formik
            initialValues={initialValue}
            validationSchema={newFeatureSchema}
            enableReinitialize
            onSubmit={handleSubmit}
          >
            {({
                values,
                errors,
                handleChange,
                handleBlur,
                handleSubmit,
                isSubmitting
              }) => (
              <>
                <Form onSubmit={handleSubmit}>
                  <FormGroup>
                    <Label for='name'>Name</Label>
                    <Input type='text'
                           name='name'
                           id='name'
                           placeholder={'Name your feature'}
                           onChange={handleChange}
                           onBlur={handleBlur}
                           value={values.name}
                           invalid={!!errors.name}
                    />
                    <FormFeedback>{errors.name}</FormFeedback>
                  </FormGroup>
                  <FormGroup>
                    <Label for="exampleText">Description</Label>
                    <Input type="textarea"
                           name="description"
                           id="description"
                           placeholder='Describe your feature'
                           onChange={handleChange}
                           onBlur={handleBlur}
                           value={values.description}/>
                    <FormFeedback>{errors.description}</FormFeedback>
                  </FormGroup>
                  <div className='float-right'>
                    <Button outline color="secondary" className='mr-2' onClick={toggleModal}>Cancel</Button>
                    <Button outline color="primary" type='submit' disabled={isSubmitting}>Save</Button>
                  </div>
                </Form>
              </>
            )}
          </Formik>
        </ModalBody>
      </Modal>
    </>
  );
};

EditFeatureModal.propTypes = {
  showModal: bool.isRequired,
  saveFeature: func.isRequired,
  toggleModal: func.isRequired,
  feature: shape({
    name: string,
    isActive: bool,
    isEnabled: bool,
    createdOn: string,
    createdBy: string,
    modifiedOn: string,
    modifiedBy: string
  })
};

export default EditFeatureModal;