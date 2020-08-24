import React from 'react';
import { array, func } from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { featureActionCreator } from '../store/actions';
import FeaturesTable from './features/FeaturesTable';

class Home extends React.PureComponent<{ features: array.isRequired, getFeatures: func.isRequired, createFeature: func.isRequired }> {
  componentDidMount() {
    this.props.getFeatures();
  }

  onSaveFeature = async feature => {
    if (!feature.id && !feature.createdOn) {
      await this.props.createFeature(feature);
    }
  };

  render() {
    return (
      <div>
        <FeaturesTable features={this.props.features} onSaveFeature={this.onSaveFeature}/>
      </div>
    );
  }
}

export default connect(
  state => ({ features: state.feature.features }),
  dispatch => bindActionCreators({ ...featureActionCreator }, dispatch)
)(Home);
