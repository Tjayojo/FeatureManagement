import React from 'react';
import { arrayOf, shape, func, string, bool } from 'prop-types';
import { Table, Button, CustomInput, Row, Col } from 'reactstrap';
import EditFeatureModal from './EditFeatureModal';
import { formatDate } from '../../utilities';
import { Link } from 'react-router-dom';
import TablePagination from './TablePagination';

const FeaturesTable = ({ features, onSaveFeature }) => {
  const [showFeatureEditForm, toggleFeatureEditForm] = React.useState(false);
  const [currentPage, setCurrentPage] = React.useState(0);
  const [pageSize, setPageSize] = React.useState(8);
  const handleNavigationClick = (e, index) => {
    e.preventDefault();
    setCurrentPage(index);
  };
  const toggleModal = () => toggleFeatureEditForm(!showFeatureEditForm);
  const getPageCount = () => features.length === 0 ? 0 : Math.ceil(features.length / pageSize);

  return (
    <>
      <Button className="float-right mb-1 btn-text" outline size='sm' color="success"
              onClick={toggleModal}>Create Feature</Button>
      <Table striped responsive hover>
        <thead>
        <tr>
          <th>Name</th>
          <th>Enabled</th>
          <th>Created By</th>
          <th>Created On</th>
          <th>Modified By</th>
          <th>Modified On</th>
          <th>Actions</th>
        </tr>
        </thead>
        <tbody>
        {features.slice(currentPage * pageSize, (currentPage + 1) * pageSize).map((feature, index) =>
          <tr key={index}>
            <td>{feature.name}</td>
            <td>{`${feature.isEnabled}`}</td>
            <td>{feature.createdBy}</td>
            <td>{formatDate(feature.createdOn)}</td>
            <td>{feature.modifiedBy}</td>
            <td>{formatDate(feature.modifiedOn)}</td>
            <td>
              <Button tag={Link} className="float-right mb-1 btn-text" outline size='sm' color="primary"
                      to={`/feature/${feature.id}`}>
                Manage
              </Button>
            </td>
          </tr>
        )}
        </tbody>
      </Table>
      <Row>
        <Col>
          <TablePagination currentPage={currentPage} handleNavigationClick={handleNavigationClick}
                           pagesCount={getPageCount()}/>
          <CustomInput
            type="select"
            id="pageSizeSelect"
            className='float-right mr-2'
            bsSize='sm'
            style={{ width: '4em' }}
            onChange={e => setPageSize(e.currentTarget.value)}
            value={pageSize}
          >
            {[5, 8, 10, 15].map((value, i) =>
              <option key={i} value={value}>{value}</option>
            )}
          </CustomInput>
        </Col>
      </Row>
      <EditFeatureModal
        showModal={showFeatureEditForm}
        toggleModal={toggleModal}
        saveFeature={onSaveFeature}
      />
    </>
  );
};

FeaturesTable.propTypes = {
  features: arrayOf(shape({
    name: string,
    isActive: bool,
    isEnabled: bool,
    createdOn: string,
    createdBy: string,
    modifiedOn: string,
    modifiedBy: string
  })).isRequired,
  onSaveFeature: func.isRequired
};

export default FeaturesTable;