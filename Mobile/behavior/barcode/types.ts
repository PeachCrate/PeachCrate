export interface Root {
  code: string
  product: Product
  status: number
  status_verbose: string
}

export interface Product {
  _id: string
  _keywords: string[]
  added_countries_tags: any[]
  additives_n: number
  additives_original_tags: string[]
  additives_tags: string[]
  allergens: string
  allergens_from_ingredients: string
  allergens_from_user: string
  allergens_hierarchy: string[]
  allergens_lc: string
  allergens_tags: string[]
  amino_acids_prev_tags: any[]
  amino_acids_tags: any[]
  brands: string
  brands_tags: string[]
  carbon_footprint_percent_of_known_ingredients: number
  categories: string
  categories_hierarchy: string[]
  categories_lc: string
  categories_old: string
  categories_properties: CategoriesProperties
  categories_properties_tags: string[]
  categories_tags: string[]
  category_properties: CategoryProperties
  checkers_tags: any[]
  ciqual_food_name_tags: string[]
  cities_tags: any[]
  code: string
  codes_tags: string[]
  compared_to_category: string
  complete: number
  completeness: number
  correctors_tags: string[]
  countries: string
  countries_beforescanbot: string
  countries_hierarchy: string[]
  countries_lc: string
  countries_tags: string[]
  created_t: number
  creator: string
  data_quality_bugs_tags: any[]
  data_quality_errors_tags: any[]
  data_quality_info_tags: string[]
  data_quality_tags: string[]
  data_quality_warnings_tags: string[]
  data_sources: string
  data_sources_tags: string[]
  debug_param_sorted_langs: string[]
  ecoscore_data: EcoscoreData
  ecoscore_grade: string
  ecoscore_score: number
  ecoscore_tags: string[]
  editors_tags: string[]
  emb_codes: string
  emb_codes_orig: string
  emb_codes_tags: any[]
  entry_dates_tags: string[]
  expiration_date: string
  food_groups: string
  food_groups_tags: string[]
  generic_name: string
  generic_name_en: string
  generic_name_es: string
  generic_name_fr: string
  generic_name_it: string
  id: string
  image_front_small_url: string
  image_front_thumb_url: string
  image_front_url: string
  image_small_url: string
  image_thumb_url: string
  image_url: string
  images: Images
  informers_tags: string[]
  ingredients: Ingredient[]
  ingredients_analysis: IngredientsAnalysis
  ingredients_analysis_tags: string[]
  ingredients_debug: string | undefined[]
  ingredients_from_or_that_may_be_from_palm_oil_n: number
  ingredients_from_palm_oil_n: number
  ingredients_from_palm_oil_tags: any[]
  ingredients_hierarchy: string[]
  ingredients_ids_debug: string[]
  ingredients_lc: string
  ingredients_n: number
  ingredients_n_tags: string[]
  ingredients_non_nutritive_sweeteners_n: number
  ingredients_original_tags: string[]
  ingredients_percent_analysis: number
  ingredients_sweeteners_n: number
  ingredients_tags: string[]
  ingredients_text: string
  ingredients_text_debug: string
  ingredients_text_en: string
  ingredients_text_es: string
  ingredients_text_es_ocr_1588083193: string
  ingredients_text_es_ocr_1588083193_result: string
  ingredients_text_es_ocr_1588087317: string
  ingredients_text_es_ocr_1588087317_result: string
  ingredients_text_fr: string
  ingredients_text_it: string
  ingredients_text_with_allergens: string
  ingredients_text_with_allergens_es: string
  ingredients_text_with_allergens_it: string
  ingredients_that_may_be_from_palm_oil_n: number
  ingredients_that_may_be_from_palm_oil_tags: any[]
  ingredients_with_specified_percent_n: number
  ingredients_with_specified_percent_sum: number
  ingredients_with_unspecified_percent_n: number
  ingredients_with_unspecified_percent_sum: number
  ingredients_without_ciqual_codes: string[]
  ingredients_without_ciqual_codes_n: number
  ingredients_without_ecobalyse_ids: string[]
  ingredients_without_ecobalyse_ids_n: number
  interface_version_created: string
  interface_version_modified: string
  known_ingredients_n: number
  labels: string
  labels_hierarchy: string[]
  labels_lc: string
  labels_old: string
  labels_tags: string[]
  lang: string
  languages: Languages
  languages_codes: LanguagesCodes
  languages_hierarchy: string[]
  languages_tags: string[]
  last_edit_dates_tags: string[]
  last_editor: string
  last_image_dates_tags: string[]
  last_image_t: number
  last_modified_by: string
  last_modified_t: number
  last_updated_t: number
  lc: string
  link: string
  main_countries_tags: any[]
  manufacturing_places: string
  manufacturing_places_tags: any[]
  max_imgid: string
  minerals_prev_tags: any[]
  minerals_tags: any[]
  misc_tags: string[]
  no_nutrition_data: string
  nova_group: number
  nova_group_debug: string
  nova_groups: string
  nova_groups_markers: NovaGroupsMarkers
  nova_groups_tags: string[]
  nucleotides_prev_tags: any[]
  nucleotides_tags: any[]
  nutrient_levels: NutrientLevels
  nutrient_levels_tags: string[]
  nutriments: Nutriments
  nutriments_estimated: NutrimentsEstimated
  nutriscore: Nutriscore
  nutriscore_2021_tags: string[]
  nutriscore_2023_tags: string[]
  nutriscore_data: NutriscoreData
  nutriscore_grade: string
  nutriscore_score: number
  nutriscore_score_opposite: number
  nutriscore_tags: string[]
  nutriscore_version: string
  nutrition_data: string
  nutrition_data_per: string
  nutrition_data_prepared: string
  nutrition_data_prepared_per: string
  nutrition_grade_fr: string
  nutrition_grades: string
  nutrition_grades_tags: string[]
  nutrition_score_beverage: number
  nutrition_score_debug: string
  nutrition_score_warning_fruits_vegetables_legumes_estimate_from_ingredients: number
  nutrition_score_warning_fruits_vegetables_legumes_estimate_from_ingredients_value: number
  nutrition_score_warning_fruits_vegetables_nuts_estimate_from_ingredients: number
  nutrition_score_warning_fruits_vegetables_nuts_estimate_from_ingredients_value: number
  obsolete: string
  obsolete_since_date: string
  origin: string
  origin_en: string
  origin_es: string
  origin_it: string
  origins: string
  origins_hierarchy: any[]
  origins_lc: string
  origins_old: string
  origins_tags: any[]
  other_nutritional_substances_tags: any[]
  packaging: string
  packaging_hierarchy: any[]
  packaging_lc: string
  packaging_materials_tags: any[]
  packaging_old: string
  packaging_recycling_tags: any[]
  packaging_shapes_tags: any[]
  packaging_tags: any[]
  packaging_text: string
  packaging_text_en: string
  packaging_text_es: string
  packaging_text_it: string
  packagings: any[]
  packagings_complete: number
  packagings_materials: PackagingsMaterials
  photographers_tags: string[]
  pnns_groups_1: string
  pnns_groups_1_tags: string[]
  pnns_groups_2: string
  pnns_groups_2_tags: string[]
  popularity_key: number
  popularity_tags: string[]
  product: Product2
  product_name: string
  product_name_en: string
  product_name_es: string
  product_name_fr: string
  product_name_it: string
  product_type: string
  purchase_places: string
  purchase_places_tags: any[]
  quantity: string
  removed_countries_tags: any[]
  rev: number
  scans_n: number
  selected_images: SelectedImages
  serving_quantity: number
  serving_quantity_unit: string
  serving_size: string
  sortkey: number
  states: string
  states_hierarchy: string[]
  states_tags: string[]
  stores: string
  stores_tags: any[]
  teams: string
  teams_tags: string[]
  traces: string
  traces_from_ingredients: string
  traces_from_user: string
  traces_hierarchy: any[]
  traces_lc: string
  traces_tags: any[]
  unique_scans_n: number
  unknown_ingredients_n: number
  unknown_nutrients_tags: any[]
  update_key: string
  vitamins_prev_tags: any[]
  vitamins_tags: any[]
  weighers_tags: any[]
}

export interface CategoriesProperties {
  "agribalyse_food_code:en": string
  "ciqual_food_code:en": string
}

export interface CategoryProperties {
  "ciqual_food_name:en": string
  "ciqual_food_name:fr": string
}

export interface EcoscoreData {
  adjustments: Adjustments
  agribalyse: Agribalyse
  grade: string
  grades: Grades
  missing: Missing
  missing_data_warning: number
  missing_key_data: number
  score: number
  scores: Scores
  status: string
}

export interface Adjustments {
  origins_of_ingredients: OriginsOfIngredients
  packaging: Packaging
  production_system: ProductionSystem
  threatened_species: ThreatenedSpecies
}

export interface OriginsOfIngredients {
  aggregated_origins: AggregatedOrigin[]
  epi_score: number
  epi_value: number
  origins_from_categories: string[]
  origins_from_origins_field: string[]
  transportation_score: number
  transportation_scores: TransportationScores
  transportation_value: number
  transportation_values: TransportationValues
  value: number
  values: Values
  warning: string
}

export interface AggregatedOrigin {
  epi_score: string
  origin: string
  percent: number
  transportation_score: number
}

export interface TransportationScores {
  ad: number
  al: number
  at: number
  ax: number
  ba: number
  be: number
  bg: number
  ch: number
  cy: number
  cz: number
  de: number
  dk: number
  dz: number
  ee: number
  eg: number
  es: number
  fi: number
  fo: number
  fr: number
  gg: number
  gi: number
  gr: number
  hr: number
  hu: number
  ie: number
  il: number
  im: number
  is: number
  it: number
  je: number
  lb: number
  li: number
  lt: number
  lu: number
  lv: number
  ly: number
  ma: number
  mc: number
  md: number
  me: number
  mk: number
  mt: number
  nl: number
  no: number
  pl: number
  ps: number
  pt: number
  ro: number
  rs: number
  se: number
  si: number
  sj: number
  sk: number
  sm: number
  sy: number
  tn: number
  tr: number
  ua: number
  uk: number
  us: number
  va: number
  world: number
  xk: number
}

export interface TransportationValues {
  ad: number
  al: number
  at: number
  ax: number
  ba: number
  be: number
  bg: number
  ch: number
  cy: number
  cz: number
  de: number
  dk: number
  dz: number
  ee: number
  eg: number
  es: number
  fi: number
  fo: number
  fr: number
  gg: number
  gi: number
  gr: number
  hr: number
  hu: number
  ie: number
  il: number
  im: number
  is: number
  it: number
  je: number
  lb: number
  li: number
  lt: number
  lu: number
  lv: number
  ly: number
  ma: number
  mc: number
  md: number
  me: number
  mk: number
  mt: number
  nl: number
  no: number
  pl: number
  ps: number
  pt: number
  ro: number
  rs: number
  se: number
  si: number
  sj: number
  sk: number
  sm: number
  sy: number
  tn: number
  tr: number
  ua: number
  uk: number
  us: number
  va: number
  world: number
  xk: number
}

export interface Values {
  ad: number
  al: number
  at: number
  ax: number
  ba: number
  be: number
  bg: number
  ch: number
  cy: number
  cz: number
  de: number
  dk: number
  dz: number
  ee: number
  eg: number
  es: number
  fi: number
  fo: number
  fr: number
  gg: number
  gi: number
  gr: number
  hr: number
  hu: number
  ie: number
  il: number
  im: number
  is: number
  it: number
  je: number
  lb: number
  li: number
  lt: number
  lu: number
  lv: number
  ly: number
  ma: number
  mc: number
  md: number
  me: number
  mk: number
  mt: number
  nl: number
  no: number
  pl: number
  ps: number
  pt: number
  ro: number
  rs: number
  se: number
  si: number
  sj: number
  sk: number
  sm: number
  sy: number
  tn: number
  tr: number
  ua: number
  uk: number
  us: number
  va: number
  world: number
  xk: number
}

export interface Packaging {
  value: number
  warning: string
}

export interface ProductionSystem {
  labels: any[]
  value: number
  warning: string
}

export interface ThreatenedSpecies {
  ingredient: string
  value: number
}

export interface Agribalyse {
  agribalyse_food_code: string
  co2_agriculture: number
  co2_consumption: number
  co2_distribution: number
  co2_packaging: number
  co2_processing: number
  co2_total: number
  co2_transportation: number
  code: string
  dqr: string
  ef_agriculture: number
  ef_consumption: number
  ef_distribution: number
  ef_packaging: number
  ef_processing: number
  ef_total: number
  ef_transportation: number
  is_beverage: number
  name_en: string
  name_fr: string
  score: number
  version: string
}

export interface Grades {
  ad: string
  al: string
  at: string
  ax: string
  ba: string
  be: string
  bg: string
  ch: string
  cy: string
  cz: string
  de: string
  dk: string
  dz: string
  ee: string
  eg: string
  es: string
  fi: string
  fo: string
  fr: string
  gg: string
  gi: string
  gr: string
  hr: string
  hu: string
  ie: string
  il: string
  im: string
  is: string
  it: string
  je: string
  lb: string
  li: string
  lt: string
  lu: string
  lv: string
  ly: string
  ma: string
  mc: string
  md: string
  me: string
  mk: string
  mt: string
  nl: string
  no: string
  pl: string
  ps: string
  pt: string
  ro: string
  rs: string
  se: string
  si: string
  sj: string
  sk: string
  sm: string
  sy: string
  tn: string
  tr: string
  ua: string
  uk: string
  us: string
  va: string
  world: string
  xk: string
}

export interface Missing {
  labels: number
  origins: number
  packagings: number
}

export interface Scores {
  ad: number
  al: number
  at: number
  ax: number
  ba: number
  be: number
  bg: number
  ch: number
  cy: number
  cz: number
  de: number
  dk: number
  dz: number
  ee: number
  eg: number
  es: number
  fi: number
  fo: number
  fr: number
  gg: number
  gi: number
  gr: number
  hr: number
  hu: number
  ie: number
  il: number
  im: number
  is: number
  it: number
  je: number
  lb: number
  li: number
  lt: number
  lu: number
  lv: number
  ly: number
  ma: number
  mc: number
  md: number
  me: number
  mk: number
  mt: number
  nl: number
  no: number
  pl: number
  ps: number
  pt: number
  ro: number
  rs: number
  se: number
  si: number
  sj: number
  sk: number
  sm: number
  sy: number
  tn: number
  tr: number
  ua: number
  uk: number
  us: number
  va: number
  world: number
  xk: number
}

export interface Images {
  "1": N1
  "2": N2
  "3": N3
  "4": N4
  "5": N5
  "6": N6
  "7": N7
  "8": N8
  front_it: FrontIt
  ingredients_es: IngredientsEs
  nutrition_es: NutritionEs
}

export interface N1 {
  sizes: Sizes
  uploaded_t: string
  uploader: string
}

export interface Sizes {
  "100": N100
  "400": N400
  full: Full
}

export interface N100 {
  h: number
  w: number
}

export interface N400 {
  h: number
  w: number
}

export interface Full {
  h: number
  w: number
}

export interface N2 {
  sizes: Sizes2
  uploaded_t: string
  uploader: string
}

export interface Sizes2 {
  "100": N1002
  "400": N4002
  full: Full2
}

export interface N1002 {
  h: number
  w: number
}

export interface N4002 {
  h: number
  w: number
}

export interface Full2 {
  h: number
  w: number
}

export interface N3 {
  sizes: Sizes3
  uploaded_t: string
  uploader: string
}

export interface Sizes3 {
  "100": N1003
  "400": N4003
  full: Full3
}

export interface N1003 {
  h: number
  w: number
}

export interface N4003 {
  h: number
  w: number
}

export interface Full3 {
  h: number
  w: number
}

export interface N4 {
  sizes: Sizes4
  uploaded_t: string
  uploader: string
}

export interface Sizes4 {
  "100": N1004
  "400": N4004
  full: Full4
}

export interface N1004 {
  h: number
  w: number
}

export interface N4004 {
  h: number
  w: number
}

export interface Full4 {
  h: number
  w: number
}

export interface N5 {
  sizes: Sizes5
  uploaded_t: number
  uploader: string
}

export interface Sizes5 {
  "100": N1005
  "400": N4005
  full: Full5
}

export interface N1005 {
  h: number
  w: number
}

export interface N4005 {
  h: number
  w: number
}

export interface Full5 {
  h: number
  w: number
}

export interface N6 {
  sizes: Sizes6
  uploaded_t: number
  uploader: string
}

export interface Sizes6 {
  "100": N1006
  "400": N4006
  full: Full6
}

export interface N1006 {
  h: number
  w: number
}

export interface N4006 {
  h: number
  w: number
}

export interface Full6 {
  h: number
  w: number
}

export interface N7 {
  sizes: Sizes7
  uploaded_t: number
  uploader: string
}

export interface Sizes7 {
  "100": N1007
  "400": N4007
  full: Full7
}

export interface N1007 {
  h: number
  w: number
}

export interface N4007 {
  h: number
  w: number
}

export interface Full7 {
  h: number
  w: number
}

export interface N8 {
  sizes: Sizes8
  uploaded_t: number
  uploader: string
}

export interface Sizes8 {
  "100": N1008
  "400": N4008
  full: Full8
}

export interface N1008 {
  h: number
  w: number
}

export interface N4008 {
  h: number
  w: number
}

export interface Full8 {
  h: number
  w: number
}

export interface FrontIt {
  angle: string
  coordinates_image_size: string
  geometry: string
  imgid: string
  normalize: string
  rev: string
  sizes: Sizes9
  white_magic: string
  x1: string
  x2: string
  y1: string
  y2: string
}

export interface Sizes9 {
  "100": N1009
  "200": N200
  "400": N4009
  full: Full9
}

export interface N1009 {
  h: number
  w: number
}

export interface N200 {
  h: number
  w: number
}

export interface N4009 {
  h: number
  w: number
}

export interface Full9 {
  h: number
  w: number
}

export interface IngredientsEs {
  angle: string
  geometry: string
  imgid: string
  normalize: string
  rev: string
  sizes: Sizes10
  white_magic: string
  x1: string
  x2: string
  y1: string
  y2: string
}

export interface Sizes10 {
  "100": N10010
  "200": N2002
  "400": N40010
  full: Full10
}

export interface N10010 {
  h: number
  w: number
}

export interface N2002 {
  h: number
  w: number
}

export interface N40010 {
  h: number
  w: number
}

export interface Full10 {
  h: number
  w: number
}

export interface NutritionEs {
  angle: number
  coordinates_image_size: string
  geometry: string
  imgid: string
  normalize: any
  rev: string
  sizes: Sizes11
  white_magic: any
  x1: string
  x2: string
  y1: string
  y2: string
}

export interface Sizes11 {
  "100": N10011
  "200": N2003
  "400": N40011
  full: Full11
}

export interface N10011 {
  h: number
  w: number
}

export interface N2003 {
  h: number
  w: number
}

export interface N40011 {
  h: number
  w: number
}

export interface Full11 {
  h: number
  w: number
}

export interface Ingredient {
  ciqual_food_code?: string
  has_sub_ingredients?: string
  id: string
  is_in_taxonomy: number
  percent?: number
  percent_estimate: number
  rank?: number
  text: string
  vegan?: string
  vegetarian?: string
  ciqual_proxy_food_code?: string
  ecobalyse_code?: string
  from_palm_oil?: string
}

export interface IngredientsAnalysis {
  "en:non-vegan": string[]
  "en:palm-oil": string[]
  "en:vegan-status-unknown": string[]
  "en:vegetarian-status-unknown": string[]
}

export interface Languages {
  "en:italian": number
  "en:spanish": number
}

export interface LanguagesCodes {
  es: number
  it: number
}

export interface NovaGroupsMarkers {
  "3": string[][]
  "4": string[][]
}

export interface NutrientLevels {
  fat: string
  salt: string
  "saturated-fat": string
  sugars: string
}

export interface Nutriments {
  carbohydrates: number
  carbohydrates_100g: number
  carbohydrates_serving: number
  carbohydrates_unit: string
  carbohydrates_value: number
  "carbon-footprint-from-known-ingredients_serving": number
  energy: number
  "energy-kcal": number
  "energy-kcal_100g": number
  "energy-kcal_serving": number
  "energy-kcal_unit": string
  "energy-kcal_value": number
  "energy-kcal_value_computed": number
  "energy-kj": number
  "energy-kj_100g": number
  "energy-kj_serving": number
  "energy-kj_unit": string
  "energy-kj_value": number
  "energy-kj_value_computed": number
  energy_100g: number
  energy_serving: number
  energy_unit: string
  energy_value: number
  fat: number
  fat_100g: number
  fat_serving: number
  fat_unit: string
  fat_value: number
  fiber_modifier: string
  "fruits-vegetables-legumes-estimate-from-ingredients_100g": number
  "fruits-vegetables-legumes-estimate-from-ingredients_serving": number
  "fruits-vegetables-nuts-estimate-from-ingredients_100g": number
  "fruits-vegetables-nuts-estimate-from-ingredients_serving": number
  "nova-group": number
  "nova-group_100g": number
  "nova-group_serving": number
  "nutrition-score-fr": number
  "nutrition-score-fr_100g": number
  proteins: number
  proteins_100g: number
  proteins_serving: number
  proteins_unit: string
  proteins_value: number
  salt: number
  salt_100g: number
  salt_serving: number
  salt_unit: string
  salt_value: number
  "saturated-fat": number
  "saturated-fat_100g": number
  "saturated-fat_serving": number
  "saturated-fat_unit": string
  "saturated-fat_value": number
  sodium: number
  sodium_100g: number
  sodium_serving: number
  sodium_unit: string
  sodium_value: number
  sugars: number
  sugars_100g: number
  sugars_serving: number
  sugars_unit: string
  sugars_value: number
}

export interface NutrimentsEstimated {
  alcohol_100g: number
  "beta-carotene_100g": number
  calcium_100g: number
  carbohydrates_100g: number
  cholesterol_100g: number
  copper_100g: number
  "energy-kcal_100g": number
  "energy-kj_100g": number
  energy_100g: number
  fat_100g: number
  fiber_100g: number
  fructose_100g: number
  galactose_100g: number
  glucose_100g: number
  iodine_100g: number
  iron_100g: number
  lactose_100g: number
  magnesium_100g: number
  maltose_100g: number
  manganese_100g: number
  "pantothenic-acid_100g": number
  phosphorus_100g: number
  phylloquinone_100g: number
  polyols_100g: number
  potassium_100g: number
  proteins_100g: number
  salt_100g: number
  "saturated-fat_100g": number
  selenium_100g: number
  sodium_100g: number
  starch_100g: number
  sucrose_100g: number
  sugars_100g: number
  "vitamin-a_100g": number
  "vitamin-b12_100g": number
  "vitamin-b1_100g": number
  "vitamin-b2_100g": number
  "vitamin-b6_100g": number
  "vitamin-b9_100g": number
  "vitamin-c_100g": number
  "vitamin-d_100g": number
  "vitamin-e_100g": number
  "vitamin-pp_100g": number
  water_100g: number
  zinc_100g: number
}

export interface Nutriscore {
  "2021": N2021
  "2023": N2023
}

export interface N2021 {
  category_available: number
  data: Data
  grade: string
  nutrients_available: number
  nutriscore_applicable: number
  nutriscore_computed: number
  score: number
}

export interface Data {
  energy: number
  energy_points: number
  energy_value: number
  fiber: number
  fiber_points: number
  fiber_value: number
  fruits_vegetables_nuts_colza_walnut_olive_oils: number
  fruits_vegetables_nuts_colza_walnut_olive_oils_points: number
  fruits_vegetables_nuts_colza_walnut_olive_oils_value: number
  is_beverage: number
  is_cheese: number
  is_fat: number
  is_water: number
  negative_points: number
  positive_points: number
  proteins: number
  proteins_points: number
  proteins_value: number
  saturated_fat: number
  saturated_fat_points: number
  saturated_fat_value: number
  sodium: number
  sodium_points: number
  sodium_value: number
  sugars: number
  sugars_points: number
  sugars_value: number
}

export interface N2023 {
  category_available: number
  data: Data2
  grade: string
  nutrients_available: number
  nutriscore_applicable: number
  nutriscore_computed: number
  score: number
}

export interface Data2 {
  components: Components
  count_proteins: number
  count_proteins_reason: string
  is_beverage: number
  is_cheese: number
  is_fat_oil_nuts_seeds: number
  is_red_meat_product: number
  is_water: number
  negative_points: number
  negative_points_max: number
  positive_nutrients: string[]
  positive_points: number
  positive_points_max: number
}

export interface Components {
  negative: Negative[]
  positive: Positive[]
}

export interface Negative {
  id: string
  points: number
  points_max: number
  unit: string
  value: number
}

export interface Positive {
  id: string
  points: number
  points_max: number
  unit: string
  value?: number
}

export interface NutriscoreData {
  components: Components2
  count_proteins: number
  count_proteins_reason: string
  grade: string
  is_beverage: number
  is_cheese: number
  is_fat_oil_nuts_seeds: number
  is_red_meat_product: number
  is_water: number
  negative_points: number
  negative_points_max: number
  positive_nutrients: string[]
  positive_points: number
  positive_points_max: number
  score: number
}

export interface Components2 {
  negative: Negative2[]
  positive: Positive2[]
}

export interface Negative2 {
  id: string
  points: number
  points_max: number
  unit: string
  value: number
}

export interface Positive2 {
  id: string
  points: number
  points_max: number
  unit: string
  value?: number
}

export interface PackagingsMaterials {}

export interface Product2 {}

export interface SelectedImages {
  front: Front
  ingredients: Ingredients
  nutrition: Nutrition
}

export interface Front {
  display: Display
  small: Small
  thumb: Thumb
}

export interface Display {
  it: string
}

export interface Small {
  it: string
}

export interface Thumb {
  it: string
}

export interface Ingredients {
  display: Display2
  small: Small2
  thumb: Thumb2
}

export interface Display2 {
  es: string
}

export interface Small2 {
  es: string
}

export interface Thumb2 {
  es: string
}

export interface Nutrition {
  display: Display3
  small: Small3
  thumb: Thumb3
}

export interface Display3 {
  es: string
}

export interface Small3 {
  es: string
}

export interface Thumb3 {
  es: string
}
