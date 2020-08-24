import React from 'react';
import { arrayOf, shape, string, func, number, bool } from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { featureActionCreator, notificationActionCreator } from '../../store/actions';
import * as Yup from 'yup';
import {
  Row,
  Col,
  Form,
  FormGroup,
  Label,
  Input,
  FormFeedback,
  Button,
  Alert,
  CustomInput,
  Spinner
} from 'reactstrap';
import { Formik } from 'formik';
import { apiPaths, supportedBrowsers } from '../../constants';
import moment from 'moment';
import { isSuccessStatusCode, Requester } from '../../utilities';
import { Link } from 'react-router-dom';
import HowTo from './HowTo';

const style = {
  fontWeight300: {
    fontWeight: 350
  },
  pageTitle: {
    fontSize: '24px',
    fontWeight: 300,
    lineHeight: 1.1,
    marginBottom: '20px'
  },
  configurationSectionTitle: {
    fontSize: '22px',
    fontWeight: 250,
    lineHeight: 1
  }
};
const Feature = ({ match: { params }, features, isLoading, getFeatures, updateFeature, showSuccess, showWarning }) => {
  const { id: featureId } = params;
  const [selectedFeature, setSelectedFeature] = React.useState({});
  const [disableRuleFields, toggleDisableRuleFields] = React.useState(selectedFeature.alwaysOn || selectedFeature.alwaysOff);
  const [fallbackFeatureSelectOptions, setFallbackFeatureSelectOptions] = React.useState({});
  const [isTestingFeature, toggleIsTestingFeature] = React.useState(false);
  const [showHowToDialog, toggleShowHowToDialog] = React.useState(true);
  React.useEffect(() => {
    const Initiate = async () => {
      if (!featureId) {
        //Todo: Do something?
        return;
      }
      let feature = features.find(f => f.id === featureId);
      const setFeature = () => {
        feature = features.find(f => f.id === featureId);
        if (feature) {
          setSelectedFeature(feature);
        }
        const fallbackFeatures = features.filter(f => f.id !== featureId).map(({ id, name }) => ({ id, name }));
        fallbackFeatures.length > 0 && setFallbackFeatureSelectOptions(fallbackFeatures);
      };
      if (features.length > 0) {
        setFeature();
      } else {
        await getFeatures().then(() => {
          setFeature();
        });
      }
    };
    Initiate().then(() => {
    });
  }, [featureId, features, selectedFeature, getFeatures]);

  const editFeatureValidationSchema = Yup.object()
    .shape({
      name: Yup.string()
        .min(5, 'Name must be at least 5 characters')
        .max(50, 'Name cannot be longer than 50 characters')
        .required('Feature name is required'),
      fallbackFeatureId: Yup.string().min(36).max(36),
      isEnabled: Yup.bool(),
      alwaysOn: Yup.bool(),
      alwaysOff: Yup.bool(),
      isArchived: Yup.bool(),
      description: Yup.string()
        .max(250, 'Description cannot be greater than 250 characters')
        .nullable(),
      timeWindow: Yup.object()
        .shape({
          isActive: Yup.bool(),
          startDate: Yup.date()
            .when('isActive', {
              is: true,
              then: Yup.date().required('Required for active time window')
            })
            .nullable(),
          endDate: Yup.date()
            .when('isActive', {
              is: true,
              then: Yup.date().required('Required for active time window')
            })
            .nullable()
        }),
      rolloutPercentage: Yup.object()
        .shape({
          isActive: Yup.boolean(),
          percentage: Yup.number()
            .when('isActive', {
              is: true,
              then: Yup.number().required('Required for active rollout percentage')
            })
            .min(0, 'Value cannot be greater than or equal to 0')
            .max(100, 'Value must be less than or equal to 100')
            .nullable()
        })
    });
  const handleSubmit = async (values, { setSubmitting }) => {
    updateFeature(values).finally(() => {
      setSubmitting(false);
    });
  };
  const testFeature = async () => {
    if (!selectedFeature.isEnabled) {
      showWarning('Enable and save feature to test');
      return;
    }
    toggleIsTestingFeature(true);
    await Requester.get(`${apiPaths.test}/${selectedFeature.name}`)
      .then(({ data, status }) => {
        if (isSuccessStatusCode(status)) {
          data ? showSuccess('Test Passed') : showWarning('Test Failed');
        }
      }).finally(() => {
        toggleIsTestingFeature(false);
      });
  };
  const initialValues = {
    id: selectedFeature.id || '',
    name: selectedFeature.name || '',
    fallbackFeatureId: selectedFeature.fallbackFeatureId || '',
    isEnabled: selectedFeature.isEnabled || false,
    alwaysOn: selectedFeature.alwaysOn || false,
    alwaysOff: selectedFeature.alwaysOff || false,
    isArchived: selectedFeature.isArchived || false,
    description: selectedFeature.description || '',
    browserRestrictions: selectedFeature.browserRestrictions || [],
    timeWindow: selectedFeature.timeWindow || {},
    rolloutPercentage: selectedFeature.rolloutPercentage || {}
  };

  return (
    <>
      {selectedFeature && (
        <div className='font-weight-light'>
          <h2 style={style.pageTitle}>
            Manage {selectedFeature.name}
            <span>
              <Button className="float-right mb-1 btn-text"
                      outline
                      size='sm'
                      color="primary"
                      onClick={testFeature}
                      disabled={isTestingFeature}>
                {isTestingFeature ? <Spinner size='sm'/> : 'Test Saved Feature'}
                </Button>
              <Button className="float-right mb-1 mr-2  btn-text"
                      outline
                      size='sm'
                      color="info"
                      onClick={() => toggleShowHowToDialog(!showHowToDialog)}>
                How To
                </Button>
            </span>
          </h2>
          <Formik
            initialValues={initialValues}
            validationSchema={editFeatureValidationSchema}
            enableReinitialize
            onSubmit={handleSubmit}
          >
            {({
                values,
                errors,
                handleChange,
                handleBlur,
                handleSubmit,
                setFieldValue,
                isSubmitting
              }) => (
              <>
                <Form onSubmit={handleSubmit}>
                  <Row>
                    <Col>
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
                    </Col>
                    <Col>
                      <FormGroup>
                        <Label for='fallbackFeatureId'>Fallback Feature</Label>
                        <CustomInput
                          type="select"
                          id="fallbackFeatureIdSelect"
                          name="fallbackFeatureId"
                          onChange={handleChange}
                          value={values.fallbackFeatureId}
                        >
                          <option value="">Select</option>
                          {fallbackFeatureSelectOptions.length > 0 && fallbackFeatureSelectOptions.map((f, i) =>
                            <option key={i} value={f.id}>{f.name}</option>
                          )}
                        </CustomInput>
                        <FormFeedback>{errors.fallbackFeatureId}</FormFeedback>
                      </FormGroup>
                    </Col>
                  </Row>
                  <Row className='text-center mb-2 mt-2'>
                    <Col>
                      <FormGroup check>
                        <Label check for='isEnabled'>
                          <Input type='checkbox'
                                 name='isEnabled'
                                 id='isEnabled'
                                 onChange={() => setFieldValue('isEnabled', !values.isEnabled)}
                                 onBlur={handleBlur}
                                 checked={values.isEnabled}
                          />
                          Is Enabled
                        </Label>
                      </FormGroup>
                    </Col>
                    <Col>
                      <FormGroup check>
                        <Label check for='alwaysOn'>
                          <Input type='checkbox'
                                 name='alwaysOn'
                                 id='alwaysOn'
                                 onChange={() => {
                                   if (values.alwaysOff || selectedFeature.alwaysOff) {
                                     setFieldValue('alwaysOff', false);
                                   }
                                   setFieldValue('alwaysOn', !values.alwaysOn);
                                   toggleDisableRuleFields(!values.alwaysOn);
                                 }}
                                 onBlur={handleBlur}
                                 checked={values.alwaysOn}
                          />
                          Is Always On
                        </Label>
                      </FormGroup>
                    </Col>
                    <Col>
                      <FormGroup check>
                        <Label check for='alwaysOff'>
                          <Input type='checkbox'
                                 name='alwaysOff'
                                 id='alwaysOff'
                                 onChange={() => {
                                   if (values.alwaysOn || selectedFeature.alwaysOn) {
                                     setFieldValue('alwaysOn', false);
                                   }
                                   setFieldValue('alwaysOff', !values.alwaysOff);
                                   toggleDisableRuleFields(!values.alwaysOff);
                                 }}
                                 onBlur={handleBlur}
                                 checked={values.alwaysOff}
                          />
                          Is Always Off
                        </Label>
                      </FormGroup>
                    </Col>
                    <Col>
                      <FormGroup check>
                        <Label check for='isArchived'>
                          <Input type='checkbox'
                                 name='isArchived'
                                 id='isArchived'
                                 onChange={() => setFieldValue('isArchived', !values.isArchived)}
                                 onBlur={handleBlur}
                                 checked={values.isArchived}
                          />
                          Is Archived
                        </Label>
                      </FormGroup>
                    </Col>
                  </Row>
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
                  {(values.alwaysOn || selectedFeature.alwaysOn) && (
                    <div className='mt-4'>
                      <Alert color="info">
                        A feature that's <strong>Always On</strong> will not evaluate other rules
                      </Alert>
                    </div>
                  )}
                  {(values.alwaysOff || selectedFeature.alwaysOff) && (
                    <div className='mt-4'>
                      <Alert color="info">
                        A feature that's <strong>Always Off</strong> will not be evaluated
                      </Alert>
                    </div>
                  )}
                  <div className='mt-5'>
                    <h5 className='font-weight-light'>Browser Restrictions</h5>
                    {values.browserRestrictions && values.browserRestrictions.length > 0 && values.browserRestrictions.map((b, i) =>
                      <FormGroup key={i} check>
                        <CustomInput type='switch'
                                     name={`values.browserRestrictions[${i}].isActive`}
                                     id={`values.browserRestrictions${i}`}
                                     label={supportedBrowsers[b.supportedBrowserId]}
                                     onChange={() => setFieldValue(`browserRestrictions[${i}].isActive`, !values.browserRestrictions[i].isActive)}
                                     onBlur={handleBlur}
                                     checked={values.browserRestrictions[i].isActive}
                                     disabled={disableRuleFields}
                        />
                      </FormGroup>
                    )}
                  </div>
                  <div className='mt-5'>
                    <h5 className='font-weight-light'>Time Window</h5>
                    <div>
                      <Row>
                        <Col>
                          <FormGroup>
                            <Label for="timeWindowStart">From</Label>
                            <Input
                              type="date"
                              name="timeWindow.start"
                              id="timeWindowStart"
                              value={moment(values.timeWindow.start).format('YYYY-MM-DD')}
                              max={values.timeWindow.end}
                              onChange={handleChange}
                              onBlur={handleBlur}
                              invalid={errors.timeWindow && !!errors.timeWindow.start}
                              disabled={disableRuleFields}
                            />
                            <FormFeedback>{errors.timeWindow && errors.timeWindow.start}</FormFeedback>
                          </FormGroup>
                        </Col>
                        <Col>
                          <FormGroup>
                            <Label for="timeWindowEnd">To</Label>
                            <Input
                              type="date"
                              name="timeWindow.end"
                              id="timeWindowEnd"
                              value={moment(values.timeWindow.end).format('YYYY-MM-DD')}
                              onChange={handleChange}
                              min={values.timeWindow.start}
                              onBlur={handleBlur}
                              invalid={errors.timeWindow && !!errors.timeWindow.end}
                              disabled={disableRuleFields}
                            />
                            <FormFeedback>{errors.timeWindow && errors.timeWindow.end}</FormFeedback>
                          </FormGroup>
                        </Col>
                      </Row>
                      <FormGroup check>
                        <Label check for='timeWindow.isActive'>
                          <Input type='checkbox'
                                 name='timeWindow.isActive'
                                 id='timeWindow.isActive'
                                 onChange={() => setFieldValue('timeWindow.isActive', !values.timeWindow.isActive)}
                                 onBlur={handleBlur}
                                 checked={values.timeWindow.isActive}
                                 disabled={disableRuleFields}
                          />
                          Is Active
                        </Label>
                      </FormGroup>
                    </div>
                  </div>
                  <div className='mt-5'>
                    <h5 className='font-weight-light'>Rollout percentage</h5>
                    <div>
                      <FormGroup>
                        <Input
                          type="number"
                          name="rolloutPercentage.percentage"
                          id="rolloutPercentage"
                          value={values.rolloutPercentage.percentage}
                          min={0}
                          max={100}
                          onChange={handleChange}
                          onBlur={handleBlur}
                          invalid={errors.rolloutPercentage && !!errors.rolloutPercentage.percentage}
                          disabled={disableRuleFields}
                        />
                        <FormFeedback>{errors.rolloutPercentage && errors.rolloutPercentage.percentage}</FormFeedback>
                      </FormGroup>
                      <FormGroup check>
                        <Label check for='rolloutPercentage.isActive'>
                          <Input type='checkbox'
                                 name='rolloutPercentage.isActive'
                                 id='rolloutPercentage.isActive'
                                 onChange={() => setFieldValue('rolloutPercentage.isActive', !values.rolloutPercentage.isActive)}
                                 onBlur={handleBlur}
                                 checked={values.rolloutPercentage.isActive}
                                 disabled={disableRuleFields}
                          />
                          Is Active
                        </Label>
                      </FormGroup>
                    </div>
                  </div>
                  {/*Todo: use feature management package to hide this*/}
                  <div className="mt-5" style={{ display: 'none' }}>
                    <h5 className="font-weight-light">Audience</h5>
                    <div>
                      Audience WIP
                    </div>
                  </div>
                  <div className='mt-2 float-right'>
                    <Button color="secondary" className='mr-2' outline size='sm' tag={Link} to="/">Cancel</Button>
                    <Button color="success" type='submit' outline size='sm'
                            disabled={isSubmitting || isLoading}>{isLoading ? <Spinner size='sm'/> : 'Save'}</Button>
                  </div>
                </Form>
              </>
            )}
          </Formik>
          {
            selectedFeature &&
            <HowTo show={showHowToDialog}
                   toggle={() => toggleShowHowToDialog(!showHowToDialog)}
                   feature={selectedFeature}/>
          }
        </div>
      )}
    </>
  );
};

Feature.propTypes = {
  features: arrayOf(shape({
    id: string.isRequired,
    name: string.isRequired,
    description: string,
    fallbackFeatureId: string,
    isEnabled: bool.isRequired,
    isArchived: bool.isRequired,
    alwaysOn: bool.isRequired,
    alwaysOff: bool.isRequired,
    timeWindow: shape({
      isActive: bool,
      startDate: string,
      endDate: string
    }),
    rolloutPercentage: shape({
      isActive: bool,
      percentage: number
    }),
    browserRestrictions: arrayOf(shape({
      id: string.isRequired,
      featureId: string.isRequired,
      isActive: bool,
      supportedBrowserId: number
    })).isRequired
  })).isRequired,
  isLoading: bool.isRequired,
  match: shape({
    params: shape({
      id: string
    }).isRequired
  }).isRequired,
  getFeatures: func.isRequired,
  updateFeature: func.isRequired,
  showSuccess: func.isRequired,
  showWarning: func.isRequired
};

export default connect(
  ({ feature }) => ({ features: feature.features, isLoading: feature.isLoading }),
  dispatch => bindActionCreators({
    ...featureActionCreator,
    showSuccess: notificationActionCreator.success,
    showWarning: notificationActionCreator.warning
  }, dispatch)
)(Feature);