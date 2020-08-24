import React from 'react';
import PropTypes from 'prop-types';
import { Pagination, PaginationItem, PaginationLink } from 'reactstrap';

const TablePagination = props => {
  const { pagesCount, currentPage, handleNavigationClick } = props;
  return (
    <Pagination className='float-right' size='sm'>
      <PaginationItem disabled={currentPage === 0}>
        <PaginationLink first href="#" onClick={e => handleNavigationClick(e, 0)}/>
      </PaginationItem>
      <PaginationItem disabled={currentPage === 0}>
        <PaginationLink previous href="#" onClick={e => handleNavigationClick(e, currentPage - 1)}/>
      </PaginationItem>
      {[...Array(pagesCount)].map((page, i) => (
        <PaginationItem active={i === currentPage} key={i}>
          <PaginationLink onClick={e => handleNavigationClick(e, i)} href="#">
            {i + 1}
          </PaginationLink>
        </PaginationItem>
      ))}
      <PaginationItem disabled={pagesCount === currentPage + 1}>
        <PaginationLink next href="#"
                        onClick={e => handleNavigationClick(e, currentPage + 1)}/>
      </PaginationItem>
      <PaginationItem disabled={pagesCount === currentPage + 1}>
        <PaginationLink last href="#" onClick={e => handleNavigationClick(e, pagesCount - 1)}/>
      </PaginationItem>
    </Pagination>
  );
};

TablePagination.propTypes = {
  pagesCount: PropTypes.number.isRequired,
  currentPage: PropTypes.number.isRequired,
  handleNavigationClick: PropTypes.func.isRequired
};

export default TablePagination;