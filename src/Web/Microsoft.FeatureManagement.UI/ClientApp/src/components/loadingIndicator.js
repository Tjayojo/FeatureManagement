import React from 'react';
import { number } from 'prop-types';
import { Spinner } from 'reactstrap';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

const style = {
  spinner:{
    cursor: 'default !important',
    pointerEvents: 'none !important'
  }
}
//Todo: Support global loading?
const LoadingIndicator = ({ inProgress }) => inProgress > 0 && <Spinner style={style.spinner} size='sm'/>;

LoadingIndicator.propTypes = {
  inProgress: number.isRequired
};

export default connect(
  ({ loading }) => ({ inProgress: loading.inProgress }),
  dispatch => bindActionCreators({}, dispatch)
)(LoadingIndicator);