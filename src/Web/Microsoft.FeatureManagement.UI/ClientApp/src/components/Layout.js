import React from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';
import Notification from './notification';

export default props => (
  <div>
    <NavMenu/>
    <Notification/>
    <Container>
      {props.children}

    </Container>
  </div>
);
