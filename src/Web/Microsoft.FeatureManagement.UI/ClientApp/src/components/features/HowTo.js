import React from 'react';
import { arrayOf, bool, func, number, shape, string } from 'prop-types';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, CustomInput } from 'reactstrap';

const HowTo = ({ show, toggle, feature }) => {
  const [tier, setTier] = React.useState('App');
  const { name } = feature;
  return (
    <Modal scrollable isOpen={show} toggle={toggle} size='lg'>
      <ModalHeader toggle={toggle}>How To Implement {name} on <CustomInput
        type="select"
        id="tierSelect"
        bsSize='sm'
        style={{ width: '5em' }}
        onChange={e => setTier(e.currentTarget.value)}
        value={tier}
      >
        {['App', 'Web'].map((value, i) =>
          <option key={i} value={value}>{value}</option>
        )}
      </CustomInput> Tier</ModalHeader>
      <ModalBody>
        <div className='mb-2'>
          <h6>1. Referencing:</h6>
          <p>Define feature flags in an enum or add new feature to existing enum</p>
          <code>
            {`public enum MyFeatureFlags { ${name} }`}
          </code>
        </div>
        <div className='mb-2'>
          <h6>2. Service Registration:</h6>
          <p>Feature flags rely on .NET Core dependency injection. We can register the feature management services using
            standard conventions.
          </p>
          <code>
            {`services.Add${tier}TierFeatureManagement(Configuration)`}
            <br/>
            {(feature.browserRestrictions && feature.browserRestrictions.filter(b => b.isActive).length > 0) &&
            <span className='ml-5'>.WithBrowserFilter()<br/></span>}
            {(feature.timeWindow && feature.timeWindow.isActive) &&
            <span className='ml-5'>.WithTimeWindowFilter()<br/></span>}
            {(feature.rolloutPercentage && feature.rolloutPercentage.isActive) &&
            <span className='ml-5'>.WithPercentageFilter()<br/></span>}
            {(feature.audience && feature.audience.isActive) &&
            <span className='ml-5'>.WithTargetingFilter()<br/></span>}
          </code>
        </div>
        <div className='mb-2'>
          <h6>3. Usage:</h6>
          <div className='ml-3 mb-2'>
            <h6>I. Inline</h6>
            <p>The basic form of feature management is checking if a feature is enabled and then performing actions
              based
              on the result. This is done through the <code>IFeatureManager</code>'s <code>IsEnabledAsync</code> method.
              <code>IFeatureManager</code> can be injected via constructor injection
            </p>
            <code>private readonly IFeatureManager _featureManager;</code>
            <br/>
            <code>{`if (await _featureManager.IsEnabledAsync(nameof(MyFeatureFlags.${name}))) { // Do something }`}</code>
          </div>
          <div className='ml-3 mb-2'>
            <h6>II. Controllers & Actions</h6>
            <p>MVC controller and actions can require that a given feature, or one of any list of features, be enabled
              in order to execute. This can be done by using a <code>FeatureGateAttribute</code></p>
            <code>{`[FeatureGate(MyFeatureFlags.${name})]`}</code>
            <br/>
            <code className='mb-2'>{`public class HomeController : Controller{ … }`}</code>
            <br/>
            <strong>Or</strong>
            <br/>
            <code>{`[FeatureGate(MyFeatureFlags.${name})]`}</code>
            <br/>
            <code className='mb-2'>{`public IActionResult Index(){ return View(); }`}</code>
          </div>
        </div>
      </ModalBody>
      <ModalFooter>
        <Button color="secondary" onClick={toggle}>Close</Button>
      </ModalFooter>
    </Modal>
  );
};

HowTo.propTypes = {
  show: bool.isRequired,
  toggle: func.isRequired,
  feature: shape({
    name: string,
    description: string,
    fallbackFeatureId: string,
    isEnabled: bool,
    isArchived: bool,
    alwaysOn: bool,
    alwaysOff: bool,
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
    })),
    audience: shape({})
  }).isRequired
};

export default HowTo;