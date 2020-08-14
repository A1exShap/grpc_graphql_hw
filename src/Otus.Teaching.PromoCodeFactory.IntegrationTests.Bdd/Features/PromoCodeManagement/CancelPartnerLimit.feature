Feature: Cancel limit
  As a PromoCodeFactory partner, 
  I need my partner manager to be able 
  to cancel my limit on promo codes
  
  @all
  Scenario: Cancel promocodes limit
    Given the partnerId is '7d994823-8226-4273-b063-1a95f3cc1df8'
    And the partner is existed
    And the partner is active
    And the partner has active limit
    When Partner manager cancel limit
    Then the limit should have CancelDate as Now